using Dapper;
using LowpriceProductsApp.Infrastructure;
using LowpriceProductsApp.Infrastructure.Services;
using LowpriceProductsApp.Presentation.Views.Windows;
using Microsoft.Extensions.Hosting;
using Microsoft.UI.Xaml;
using System;

namespace LowpriceProductsApp.Presentation;

public partial class App : Application
{
    public static IServiceProvider Provider
        => CreateHostBuilder().Build().Services;

    private Window? _window;

    public App()
    {
        InitializeComponent();
        AddSqlMapperHandlers();
    }

    protected override void OnLaunched(Microsoft.UI.Xaml.LaunchActivatedEventArgs args)
    {
        _window = new MainWindow();
        _window.Activate();
    }

    private static IHostBuilder CreateHostBuilder()
        => Host.CreateDefaultBuilder()
                .ConfigureServices((context, services) =>
                {
                    services.AddPresentation(context.Configuration);
                    services.AddInfrastructure(context.Configuration);
                });

    private static void AddSqlMapperHandlers()
    {
        SqlMapper.AddTypeHandler(new GuidTypeHandler());
        SqlMapper.AddTypeHandler(new NullableGuidTypeHandler());
    }
}
