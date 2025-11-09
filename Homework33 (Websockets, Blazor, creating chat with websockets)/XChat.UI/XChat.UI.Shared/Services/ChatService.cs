using Microsoft.Extensions.Configuration;
using System.Net.Http.Json;
using System.Net.WebSockets;
using System.Text;
using System.Text.Json;
using XChat.UI.Shared.Dto.Message;
using XChat.UI.Shared.Models;

namespace XChat.UI.Shared.Services;

public class ChatService : IAsyncDisposable
{
    private readonly IHttpClientFactory _httpFactory;
    private readonly IConfiguration _configuration;
    private readonly JsonSerializerOptions _jsonOptions = new() { PropertyNameCaseInsensitive = true };
    private readonly Dictionary<string, List<MessageDto>> _chatMessages = [];
    private readonly Dictionary<string, ClientWebSocket> _sockets = [];

    public event Action? OnMessagesUpdated;
    public bool IsLoadingOlder { get; private set; }

    private const int PageSize = 10;
    private const int LoadingDelayMs = 400;

    public ChatService(IHttpClientFactory httpFactory, IConfiguration configuration)
    {
        _httpFactory = httpFactory;
        _configuration = configuration;
    }

    public async Task<List<Room>> GetRoomsForUserAsync(Guid userId)
    {
        var client = _httpFactory.CreateClient("ApiClient");
        var rooms = await client.GetFromJsonAsync<List<Room>>($"api/rooms?userId={userId}");
        return rooms ?? [];
    }

    public async Task<Room?> CreateGroupRoomAsync(string name, Guid ownerUserId)
    {
        var client = _httpFactory.CreateClient("ApiClient");
        var payload = new { Name = name, OwnerId = ownerUserId };
        var response = await client.PostAsJsonAsync("api/rooms", payload);
        if (!response.IsSuccessStatusCode)
            return null;
        var room = await response.Content.ReadFromJsonAsync<Room>();
        return room;
    }

    public async Task<Room?> CreatePersonalRoomAsync(string ownerUsername, string anotherUsername)
    {
        var client = _httpFactory.CreateClient("ApiClient");
        var payload = new { RequestUsername = ownerUsername, AnotherUsername = anotherUsername };
        var response = await client.PostAsJsonAsync("api/rooms/personal", payload);
        if (!response.IsSuccessStatusCode)
            return null;
        var room = await response.Content.ReadFromJsonAsync<Room>();
        return room;
    }

    public async Task<bool> AddUserToRoomAsync(Guid roomId, string username)
    {
        var client = _httpFactory.CreateClient("ApiClient");
        var payload = new { Username = username };
        var response = await client.PostAsJsonAsync($"api/rooms/add-user?roomId={roomId}", payload);
        return response.IsSuccessStatusCode;
    }

    public IReadOnlyList<MessageDto> MessagesFor(string chatId)
        => _chatMessages.TryGetValue(chatId, out var list) ? list : [];

    public async Task ConnectAsync(string? chatId)
    {
        if (string.IsNullOrWhiteSpace(chatId))
            return;
        if (_sockets.ContainsKey(chatId) && _sockets[chatId].State == WebSocketState.Open)
            return;
        var uri = new Uri($"{_configuration["Api:WsUrl"]}?chatId={chatId}");
        var socket = new ClientWebSocket();
        await socket.ConnectAsync(uri, CancellationToken.None);
        _sockets[chatId] = socket;
        _ = ListenAsync(chatId, socket);
    }

    private async Task ListenAsync(string chatId, ClientWebSocket socket)
    {
        var buffer = new byte[4096];
        while (socket.State == WebSocketState.Open)
        {
            var result = await socket.ReceiveAsync(buffer, CancellationToken.None);
            if (result.MessageType == WebSocketMessageType.Close)
                break;
            var json = Encoding.UTF8.GetString(buffer, 0, result.Count);
            var message = JsonSerializer.Deserialize<MessageDto>(json, _jsonOptions);
            if (message != null)
            {
                if (!_chatMessages.ContainsKey(chatId))
                    _chatMessages[chatId] = [];
                _chatMessages[chatId].Add(message);
                OnMessagesUpdated?.Invoke();
            }
        }
    }

    public async Task SendMessageAsync(string? chatId, string? username, string text)
    {
        if (string.IsNullOrWhiteSpace(chatId) || string.IsNullOrWhiteSpace(username))
            return;
        var client = _httpFactory.CreateClient("ApiClient");
        var payload = new { UserName = username, Text = text };
        var response = await client.PostAsJsonAsync($"/api/messages?chatId={chatId}", payload);
        if (!response.IsSuccessStatusCode)
            throw new HttpRequestException($"Failed to send message: {response.StatusCode}");
    }

    public async Task LoadMessagesAsync(string? chatId)
    {
        if (string.IsNullOrWhiteSpace(chatId))
            return;
        var client = _httpFactory.CreateClient("ApiClient");
        var recent = await client.GetFromJsonAsync<List<MessageDto>>($"api/messages/recent?chatId={chatId}&count={PageSize}");
        if (recent != null)
        {
            _chatMessages[chatId] = recent.OrderBy(m => m.CreatedAt).ToList();
            OnMessagesUpdated?.Invoke();
        }
    }

    public async Task LoadOlderMessagesAsync(string? chatId, DateTimeOffset before)
    {
        if (string.IsNullOrWhiteSpace(chatId))
            return;
        IsLoadingOlder = true;
        try
        {
            var client = _httpFactory.CreateClient("ApiClient");
            var older = await client.GetFromJsonAsync<List<MessageDto>>(
                $"api/messages/history?chatId={chatId}&before={before.ToUnixTimeMilliseconds()}&count={PageSize}");
            if (older?.Any() == true)
            {
                if (!_chatMessages.ContainsKey(chatId))
                    _chatMessages[chatId] = new List<MessageDto>();
                _chatMessages[chatId].InsertRange(0, older.OrderBy(m => m.CreatedAt));
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
        foreach (var socket in _sockets.Values)
        {
            if (socket.State == WebSocketState.Open)
                await socket.CloseAsync(WebSocketCloseStatus.NormalClosure, "Closed by client", CancellationToken.None);
            socket.Dispose();
        }
        _sockets.Clear();
    }
}
