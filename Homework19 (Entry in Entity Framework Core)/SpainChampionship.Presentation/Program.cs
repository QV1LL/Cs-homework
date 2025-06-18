using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SpainChampionship.Infrastructure.Persistence;
using SpainChampionship.Infrastructure.Services;
using SpainChampionship.Presentation.Services;

namespace SpainChampionship.Presentation;

internal static class Program
{
    private static IServiceProvider _services;

    private static async Task Main(string[] args)
    {
        var host = CreateHost(args);
        host.RunAsync();
        _services = host.Services;

        await TestApp();
    }

    private static async Task TestApp()
    {
        var context = _services.GetRequiredService<SpainChampionshipContext>();

        await context.Database.EnsureDeletedAsync();
        await context.Database.EnsureCreatedAsync();

        var dataSeeder = new DataSeeder(context);
        dataSeeder.SeedTeams();

        PrintService.PrintTeams(context.Teams);
    }

    private static IHost CreateHost(string[] args)
        => Host.CreateDefaultBuilder(args)
               .ConfigureServices((context, services) =>
                    services.AddDbContext<SpainChampionshipContext>())
               .Build();
}