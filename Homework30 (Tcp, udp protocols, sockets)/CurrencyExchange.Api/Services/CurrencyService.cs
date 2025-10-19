using FluentResults;

namespace CurrencyExchange.Api.Services;

using CurrencyRate = Dictionary<string, double>;

internal class CurrencyService
{
    private readonly CurrencyRate _currencyRate;

    public CurrencyService(string currencyRateFileName)
    {
        var task = ParseCurrencyRateService.ParseAsync(currencyRateFileName);
        _currencyRate = task.Result;
    }

    public Result<double> GetExchangeRate(string fromCurrency, string toCurrency)
    {
        if (!_currencyRate.ContainsKey(fromCurrency))
            return Result.Fail(new Error($"Unknown currency code: {fromCurrency}"));

        if (!_currencyRate.ContainsKey(toCurrency))
            return Result.Fail(new Error($"Unknown currency code: {toCurrency}"));

        double fromRate = _currencyRate[fromCurrency];
        double toRate = _currencyRate[toCurrency];

        return fromRate / toRate;
    }
}
