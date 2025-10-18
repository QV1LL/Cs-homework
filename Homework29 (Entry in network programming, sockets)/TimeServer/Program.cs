using TimeServer.Services;

namespace TimeServer;

internal static class Program
{
    static async Task Main(string[] args)
    {
        var service = new ServerService();
        await service.StartAsync();
    }
}
