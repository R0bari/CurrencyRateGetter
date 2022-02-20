using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Memory;
using RateGetters.Rates.Models;
using RateGetters.Rates.Models.Enums;
using RateGetters.Rates.Services.Interfaces;

namespace RateGetters.Rates.Services;

public class CachedCbrRateService : IRateService
{
    private readonly CbrRateService _cbrRateService = new();

    private readonly IMemoryCache _cache =
        new MemoryCache(
            new MemoryCacheOptions());

    private readonly PeriodRateList _emptyPeriodRateList = new(new List<RateForDate>());

    public async Task<RateForDate> GetRateAsync(DateTime date, CurrencyCodesEnum code)
    {
        if (_cache.TryGetValue((code, date, DateTime.MinValue), out RateForDate rateForDate))
        {
            return await Task.FromResult(rateForDate);
        }

        var result = await _cbrRateService
            .GetRateAsync(date, code)
            .ConfigureAwait(false);
        if (result != RateForDate.Empty)
        {
            _cache.Set(
                (code, date, DateTime.MinValue),
                result,
                new MemoryCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromDays(1)
                });
        }

        return await Task.FromResult(result);
    }

    public async Task<PeriodRateList> GetRatesForPeriodAsync(DateTime first, DateTime second, CurrencyCodesEnum code)
    {
        if (_cache.TryGetValue(
                first < second
                    ? (code, first, second)
                    : (code, second, first),
                out PeriodRateList periodRateList))
        {
            return await Task.FromResult(periodRateList);
        }

        var result = await _cbrRateService
            .GetRatesForPeriodAsync(first, second, code)
            .ConfigureAwait(false);
        if (!Equals(result, _emptyPeriodRateList))
        {
            _cache.Set(
                (code, first, second),
                result,
                new MemoryCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromDays(1)
                });
        }

        return await Task.FromResult(result);
    }
}
