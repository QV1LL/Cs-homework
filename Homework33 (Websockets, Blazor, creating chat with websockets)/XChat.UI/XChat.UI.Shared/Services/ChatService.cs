using Microsoft.Extensions.Configuration;
using System.Net.Http.Json;
using System.Net.WebSockets;
using System.Text;
using System.Text.Json;
using XChat.UI.Shared.Dto.Message;

namespace XChat.UI.Shared.Services;

public class ChatService : IAsyncDisposable
{
    private readonly HttpClient _http;
    private readonly ClientWebSocket _socket = new();
    private readonly IConfiguration _configuration;
    private readonly JsonSerializerOptions _jsonOptions = new() { PropertyNameCaseInsensitive = true };

    private readonly List<MessageDto> _messages = new();
    public IReadOnlyList<MessageDto> Messages => _messages;

    public bool IsLoadingOlder { get; private set; }

    public event Action? OnMessagesUpdated;

    private const int PageSize = 8;
    private const int LoadingDelayMs = 500;

    public ChatService(IHttpClientFactory httpFactory, IConfiguration configuration)
    {
        _http = httpFactory.CreateClient("ApiClient");
        _configuration = configuration;
    }

    public async Task ConnectAsync()
    {
        if (_socket.State == WebSocketState.Open)
            return;

        var uri = new Uri(_configuration?["Api:WsUrl"] ?? string.Empty);
        await _socket.ConnectAsync(uri, CancellationToken.None);

        _ = ListenAsync();
    }

    private async Task ListenAsync()
    {
        var buffer = new byte[4096];
        while (_socket.State == WebSocketState.Open)
        {
            var result = await _socket.ReceiveAsync(buffer, CancellationToken.None);
            if (result.MessageType == WebSocketMessageType.Close)
                break;

            var json = Encoding.UTF8.GetString(buffer, 0, result.Count);
            var message = JsonSerializer.Deserialize<MessageDto>(json, _jsonOptions);
            if (message != null)
            {
                _messages.Add(message);
                OnMessagesUpdated?.Invoke();
            }
        }
    }

    public async Task SendMessageAsync(string username, string text)
    {
        var request = new { UserName = username, Text = text };
        var response = await _http.PostAsJsonAsync("/api/messages", request);

        if (!response.IsSuccessStatusCode)
        {
            throw new HttpRequestException($"Failed to send message: {response.StatusCode}");
        }
    }

    public async Task LoadRecentMessagesAsync()
    {
        var recent = await _http.GetFromJsonAsync<List<MessageDto>>($"api/messages/recent?count={PageSize}");
        if (recent != null)
        {
            _messages.Clear();
            _messages.AddRange(recent.OrderBy(m => m.CreatedAt));
            OnMessagesUpdated?.Invoke();
        }
    }

    public async Task LoadOlderMessagesAsync(DateTimeOffset before)
    {
        IsLoadingOlder = true;
        try
        {
            var older = await _http.GetFromJsonAsync<List<MessageDto>>($"api/messages/history?before={before.ToUnixTimeMilliseconds()}&count={PageSize}");
            if (older?.Any() == true)
            {
                _messages.InsertRange(0, older.OrderBy(m => m.CreatedAt));
                OnMessagesUpdated?.Invoke();
            }
            await Task.Delay(LoadingDelayMs);
        }
        finally
        {
            IsLoadingOlder = false;
        }
    }

    public async ValueTask DisposeAsync()
    {
        if (_socket.State == WebSocketState.Open)
            await _socket.CloseAsync(WebSocketCloseStatus.NormalClosure, "Closed by client", CancellationToken.None);

        _socket.Dispose();
    }
}