using Microsoft.Extensions.Configuration;
using System.Reflection;
using System.Text;
using XChat.UI.Shared.Extensions;

namespace XChat.UI
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                });

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

            builder.Services.AddMauiBlazorWebView();
            builder.Services.AddXChatServices(config);

            return builder.Build();
        }
    }
}
