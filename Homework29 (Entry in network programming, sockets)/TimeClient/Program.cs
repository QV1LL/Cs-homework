using System.Net.Sockets;
using System.Text;

namespace TimeClient;

internal static class Program
{
    private const string IP_ADRESS = "localhost";
    private const int PORT = 62345;

    static async Task Main()
    {
        var client = new Socket(
            AddressFamily.InterNetwork, 
            SocketType.Stream, 
            ProtocolType.Tcp
        );

        await client.ConnectAsync(IP_ADRESS, PORT);

        var buffer = new byte[1024];
        var bytesReceived = await client.ReceiveAsync(buffer);
        var response = Encoding.UTF8.GetString(buffer, 0, bytesReceived);

        Console.WriteLine($"Server [ip - {IP_ADRESS}] response at ({DateTimeOffset.Now:HH:mm:ss}): {response}");
        Console.WriteLine("Enter a command (time, date)");

        var input = Console.ReadLine() ?? string.Empty;

        await client.SendAsync(Encoding.UTF8.GetBytes(input));
        bytesReceived = await client.ReceiveAsync(buffer);
        response = Encoding.UTF8.GetString(buffer, 0, bytesReceived);
        Console.WriteLine($"Server [ip - {IP_ADRESS}] response at ({DateTimeOffset.Now:HH:mm:ss}): {response}");
        Console.ReadKey();
    }
}
