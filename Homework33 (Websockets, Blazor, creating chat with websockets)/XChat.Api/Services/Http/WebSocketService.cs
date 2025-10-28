using Microsoft.Extensions.Logging;
using System.Collections.Concurrent;
using System.Net.WebSockets;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace XChat.Api.Services.Http;

public class WebSocketService
{
    private readonly ConcurrentBag<WebSocket> _clients = new();
    private readonly ILogger<WebSocketService> _logger;

    public WebSocketService(ILogger<WebSocketService> logger)
    {
        _logger = logger;
    }

    public void AddClientAsync(WebSocket webSocket)
    {
        _clients.Add(webSocket);
        _logger.LogInformation("WebSocket client connected. Total: {Count}", _clients.Count);

        _ = HandleClientAsync(webSocket);
    }

    private async Task HandleClientAsync(WebSocket webSocket)
    {
        var buffer = new byte[1024 * 4];
        try
        {
            while (webSocket.State == WebSocketState.Open)
            {
                var result = await webSocket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);
                if (result.MessageType == WebSocketMessageType.Close)
                {
                    await webSocket.CloseAsync(WebSocketCloseStatus.NormalClosure, "Closing", CancellationToken.None);
                    break;
                }
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "WebSocket client error");
        }
        finally
        {
            _clients.TryTake(out webSocket);
            _logger.LogInformation("WebSocket client disconnected. Total: {Count}", _clients.Count);
        }
    }

    public async Task BroadcastAsync<T>(T message)
    {
        try
        {
            var json = JsonSerializer.Serialize(message);
            var bytes = Encoding.UTF8.GetBytes(json);

            var tasks = new List<Task>();

            foreach (var client in _clients)
            {
                if (client.State == WebSocketState.Open)
                {
                    tasks.Add(client.SendAsync(new ArraySegment<byte>(bytes), WebSocketMessageType.Text, true, CancellationToken.None));

                    _logger.LogInformation($"Broadcast message");
                }
            }

            await Task.WhenAll(tasks);
            _logger.LogInformation("Broadcasted message to {Count} clients", tasks.Count);
        }
        catch (Exception ex) 
        {
            _logger.LogError($"Failed to broadcast: {ex.Message}", ex);
        }
    }

    public void RemoveClient(WebSocket webSocket)
    {
        _clients.TryTake(out webSocket);
    }
}