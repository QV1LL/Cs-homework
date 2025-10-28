using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using XChat.UI.Shared.Extensions;

namespace XChat.UI.Web.Client
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);

            var services = builder.Services;

            services.AddXChatServices();

            await builder.Build().RunAsync();
        }
    }
}
