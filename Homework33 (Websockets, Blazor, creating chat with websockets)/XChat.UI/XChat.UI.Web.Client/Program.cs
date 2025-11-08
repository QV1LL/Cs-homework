using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using System.Reflection;
using XChat.UI.Shared.Extensions;

namespace XChat.UI.Web.Client
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);

            var assembly = Assembly.GetAssembly(typeof(Shared._Imports));
            var resourceName = assembly!.GetManifestResourceNames()
                                        .Single(
                                            n => n.EndsWith("appsettings.json"));
            using var stream = assembly!.GetManifestResourceStream(resourceName);

            var config = new ConfigurationBuilder()
                .AddJsonStream(stream!)
                .Build();

            builder.Configuration.AddConfiguration(config);
            builder.Services.AddSingleton<IConfiguration>(config);

            builder.Services.AddXChatServices(config);

            await builder.Build().RunAsync();
        }
    }
}
