using GamesApp.Infrastructure;
using GamesApp.Infrastructure.Persistence;
using GamesApp.Infrastructure.Services;
using GamesApp.Presentation.Views.Windows;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.UI.Xaml;
using System;
using System.Threading.Tasks;

namespace GamesApp.Presentation;

public partial class App : Application
{
    public static IServiceProvider Provider
        => CreateHostBuilder().Build().Services;

    private Window? _window;

    public App()
    {
        InitializeComponent();

        Task.Run(async () => await SeedDatabase());
    }

    protected override void OnLaunched(Microsoft.UI.Xaml.LaunchActivatedEventArgs args)
    {
        _window = Provider.GetRequiredService<MainWindow>();
        _window.Activate();
    }

    private async static Task SeedDatabase()
    {
        await GamesAppContextSeeder.SeedAsync(Provider.GetRequiredService<GamesAppContext>());
    }

    private static IHostBuilder CreateHostBuilder()
        => Host.CreateDefaultBuilder()
                .ConfigureServices((context, services) =>
                {
                    services.AddPresentation(context.Configuration);
                    services.AddInfrastructure(context.Configuration);
                });
}
