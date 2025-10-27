using Microsoft.Extensions.Configuration;
using System.Net.WebSockets;
using System.Text;
using System.Text.Json;
using XChat.Shared.Models;

namespace XChat.Shared.Services;

public class ChatService
{
    private ClientWebSocket? _webSocket;
    private readonly string _webSocketUrl;
    public event Action<Message>? OnMessageReceived;

    public ChatService(IConfiguration config)
    {
        _webSocketUrl = config["BackendWebSocketUrl"] ?? "ws://localhost:8080/chat";
    }

    public async Task ConnectAsync(string token)
    {
        _webSocket = new ClientWebSocket();
        var fullUrl = $"{_webSocketUrl}?token={token}";
        await _webSocket.ConnectAsync(new Uri(fullUrl), CancellationToken.None);
        _ = Task.Run(ReceiveMessagesAsync);
    }

    private async Task ReceiveMessagesAsync()
    {
        var buffer = new byte[4096];
        while (_webSocket?.State == WebSocketState.Open)
        {
            var result = await _webSocket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);
            if (result.MessageType == WebSocketMessageType.Text)
            {
                var messageJson = Encoding.UTF8.GetString(buffer, 0, result.Count);
                var message = JsonSerializer.Deserialize<Message>(messageJson);
                if (message != null)
                {
                    OnMessageReceived?.Invoke(message);
                }
            }
        }
    }

    public async Task SendMessageAsync(Message message)
    {
        if (_webSocket?.State == WebSocketState.Open)
        {
            var json = JsonSerializer.Serialize(message);
            var bytes = Encoding.UTF8.GetBytes(json);
            await _webSocket.SendAsync(new ArraySegment<byte>(bytes), WebSocketMessageType.Text, true, CancellationToken.None);
        }
    }

    public async Task DisconnectAsync()
    {
        if (_webSocket != null)
        {
            await _webSocket.CloseAsync(WebSocketCloseStatus.NormalClosure, "Disconnected", CancellationToken.None);
            _webSocket.Dispose();
            _webSocket = null;
        }
    }
}
