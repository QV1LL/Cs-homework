using CurrencyExchange.Api.Helpers;
using CurrencyExchange.Api.Services;

namespace CurrencyExchange.Api;

internal static class Program
{
    private const string CURRENCIES_RATE_FILE_NAME = "currencies_rate.json";
    private const string CONFIGURATION_FILE_NAME = "server_config.json";

    public static async Task Main()
    {
        var configurationResult = Configuration.Load(CONFIGURATION_FILE_NAME);

        if (configurationResult.IsFailed)
            LogService.Error($"Failed to load configuration file {CONFIGURATION_FILE_NAME}");

        var currencyService = new CurrencyService(CURRENCIES_RATE_FILE_NAME);
        using var server = new ServerHostService(currencyService, configurationResult.Value);
        await server.Run();
    }
}
