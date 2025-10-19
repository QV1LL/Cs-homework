using System.Net.Sockets;

namespace CurrencyExchange.Client;

internal static class Program
{
    private static async Task Main()
    {
        Console.WriteLine("TCP Client");

        Console.Write("Server IP (default 127.0.0.1): ");
        string ip = Console.ReadLine();
        if (string.IsNullOrWhiteSpace(ip))
            ip = "127.0.0.1";

        Console.Write("Server Port (default 62345): ");
        string portStr = Console.ReadLine();
        int port = 62345;
        if (!string.IsNullOrWhiteSpace(portStr) && int.TryParse(portStr, out var parsedPort))
            port = parsedPort;

        try
        {
            using var client = new TcpClient();
            await client.ConnectAsync(ip, port);
            Console.WriteLine($"Connected to server {ip}:{port}");

            using var stream = client.GetStream();
            using var reader = new StreamReader(stream);
            using var writer = new StreamWriter(stream) { AutoFlush = true };

            var readTask = Task.Run(async () =>
            {
                string? serverLine;
                while ((serverLine = await reader.ReadLineAsync()) != null)
                {
                    Console.WriteLine($"[SERVER] {serverLine}");
                }
            });

            while (true)
            {
                string? input = Console.ReadLine();

                if (string.IsNullOrWhiteSpace(input))
                    continue;

                await writer.WriteLineAsync(input);

                if (string.Equals(input, "exit", StringComparison.OrdinalIgnoreCase))
                    break;
            }

            await readTask;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
        }

        Console.WriteLine("Client closed. Press any key to exit.");
        Console.ReadKey();
    }
}
