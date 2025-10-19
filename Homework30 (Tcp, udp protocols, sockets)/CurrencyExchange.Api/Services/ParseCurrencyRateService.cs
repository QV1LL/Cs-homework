using System.Text.Json;

namespace CurrencyExchange.Api.Services;

using CurrencyRate = Dictionary<string, double>;

internal static class ParseCurrencyRateService
{
    public static async Task<CurrencyRate> ParseAsync(string fileName)
    {
        if (!File.Exists(fileName)) return [];

        var fileStream = File.OpenRead(fileName);

        return await JsonSerializer.DeserializeAsync<CurrencyRate>(fileStream) ?? [];
    } 
}
