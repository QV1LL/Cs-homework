using System.Net;
using System.Net.Sockets;
using System.Text;

namespace TimeServer.Services;

internal class ServerService
{
    private readonly IPEndPoint _endPoint;

    public ServerService(int port = 62345)
    {
        _endPoint = new IPEndPoint(IPAddress.Any, port);
    }

    public async Task StartAsync()
    {
        using var server = new Socket(
            AddressFamily.InterNetwork, 
            SocketType.Stream, 
            ProtocolType.Tcp
        );

        server.Bind(_endPoint);
        server.Listen(5);

        var client = await server.AcceptAsync();

        var response = "connection established";
        await client.SendAsync(Encoding.UTF8.GetBytes(response));

        var buffer = new byte[1024];
        var bytesReceived = await client.ReceiveAsync(buffer);
        var command = Encoding.UTF8.GetString(buffer, 0, bytesReceived ).ToLower();

        response = command switch
        {
            "date" => DateTimeOffset.Now.ToString("yyyy-MM-dd"),
            "time" => DateTimeOffset.Now.ToString("HH:mm:ss"),
            _ => "Unknown command"
        };

        await client.SendAsync(Encoding.UTF8.GetBytes(response));
    }
}
