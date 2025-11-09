using Microsoft.Extensions.Logging;
using System.Collections.Concurrent;
using System.Net.WebSockets;
using System.Text;
using System.Text.Json;

namespace XChat.Api.Services.Http;

public class WebSocketService
{
    private readonly ConcurrentDictionary<Guid, ConcurrentBag<WebSocket>> _roomClients = new();
    private readonly ILogger<WebSocketService> _logger;

    public WebSocketService(ILogger<WebSocketService> logger)
    {
        _logger = logger;
    }

    public void AddClient(Guid roomId, WebSocket socket)
    {
        var clients = _roomClients.GetOrAdd(roomId, _ => []);
        clients.Add(socket);
        _logger.LogInformation("Client added to room {RoomId}. Total clients: {Count}", roomId, clients.Count);
        _ = HandleClientAsync(roomId, socket);
    }

    private async Task HandleClientAsync(Guid roomId, WebSocket socket)
    {
        var buffer = new byte[1024 * 4];
        try
        {
            while (socket.State == WebSocketState.Open)
            {
                var result = await socket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);
                if (result.MessageType == WebSocketMessageType.Close)
                {
                    await socket.CloseAsync(WebSocketCloseStatus.NormalClosure, "Closing", CancellationToken.None);
                    break;
                }
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "WebSocket error in room {RoomId}", roomId);
        }
        finally
        {
            RemoveClient(roomId, socket);
        }
    }

    public async Task BroadcastAsync<T>(Guid roomId, T message)
    {
        if (!_roomClients.TryGetValue(roomId, out var clients) || clients.Count == 0)
            return;

        try
        {
            var json = JsonSerializer.Serialize(message);
            var bytes = Encoding.UTF8.GetBytes(json);
            var segment = new ArraySegment<byte>(bytes);

            var tasks = clients
                .Where(c => c.State == WebSocketState.Open)
                .Select(c => c.SendAsync(segment, WebSocketMessageType.Text, true, CancellationToken.None))
                .ToList();

            await Task.WhenAll(tasks);
            _logger.LogInformation("Broadcasted message to {Count} clients in room {RoomId}", tasks.Count, roomId);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to broadcast message in room {RoomId}", roomId);
        }
    }

    public void RemoveClient(Guid roomId, WebSocket socket)
    {
        if (_roomClients.TryGetValue(roomId, out var clients))
        {
            var updated = new ConcurrentBag<WebSocket>(clients.Where(s => s != socket));
            _roomClients[roomId] = updated;
            _logger.LogInformation("Client removed from room {RoomId}. Remaining: {Count}", roomId, updated.Count);
        }
    }
}
