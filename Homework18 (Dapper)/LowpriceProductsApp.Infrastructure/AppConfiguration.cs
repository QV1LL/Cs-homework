using Microsoft.Extensions.Configuration;
using System;

namespace LowpriceProductsApp.Infrastructure;

internal static class AppConfiguration
{
    private static readonly IConfigurationRoot _configuration;

    static AppConfiguration()
    {
        _configuration = new ConfigurationBuilder()
            .SetBasePath(AppContext.BaseDirectory)
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .Build();
    }

    public static string AppName => _configuration["AppInfo:AppName"];
    public static string DbFileName => _configuration["AppInfo:DbFileName"];
    public static string ProviderName => _configuration["AppInfo:ProviderName"];
}
