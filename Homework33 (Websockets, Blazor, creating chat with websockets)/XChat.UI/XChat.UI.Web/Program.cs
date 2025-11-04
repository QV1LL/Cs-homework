using System.Reflection;
using XChat.UI.Shared.Extensions;
using XChat.UI.Shared.Services;
using XChat.UI.Web.Components;

namespace XChat.UI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddRazorComponents()
                .AddInteractiveServerComponents()
                .AddInteractiveWebAssemblyComponents();

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

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseWebAssemblyDebugging();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();

            app.UseStaticFiles();
            app.UseAntiforgery();

            app.MapRazorComponents<App>()
                .AddInteractiveServerRenderMode()
                .AddInteractiveWebAssemblyRenderMode()
                .AddAdditionalAssemblies(
                    typeof(XChat.UI.Shared._Imports).Assembly,
                    typeof(XChat.UI.Web.Client._Imports).Assembly);

            app.Run();
        }
    }
}
