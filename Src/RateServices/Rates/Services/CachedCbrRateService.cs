using System;
using System.Collections.Generic;
using Microsoft.Extensions.Caching.Memory;
using RateGetters.Rates.Models;
using RateGetters.Rates.Models.Enums;
using RateGetters.Rates.Services.Interfaces;

namespace RateGetters.Rates.Services
{
    public class CachedCbrRateService : IRateService
    {
        private readonly CbrRateService _cbrRateService = new();
        private readonly IMemoryCache _cache;

        private readonly RateForDate _emptyRateForDate =
            new(
                new Rate(CurrencyCodesEnum.None, 0m),
                DateTime.MinValue);

        private readonly PeriodRateList _emptyPeriodRateList = new(new List<RateForDate>());

        public CachedCbrRateService(IMemoryCache cache) => _cache = cache;

        public RateForDate GetRate(DateTime dateTime, CurrencyCodesEnum code)
        {
            if (_cache.TryGetValue((dateTime, DateTime.MinValue), out RateForDate rateForDate))
            {
                return rateForDate;
            }

            var result = _cbrRateService.GetRate(dateTime, code);
            if (result != _emptyRateForDate)
            {
                _cache.Set(
                    (dateTime, DateTime.MinValue),
                    result,
                    new MemoryCacheEntryOptions
                    {
                        AbsoluteExpirationRelativeToNow = TimeSpan.FromDays(1)
                    });
            }

            return result;
        }

        public PeriodRateList GetRatesForPeriod(DateTime first, DateTime second, CurrencyCodesEnum code)
        {
            if (_cache.TryGetValue((first, second), out PeriodRateList periodRateList)
                ||
                _cache.TryGetValue((second, first), out periodRateList))
            {
                return periodRateList;
            }

            var result = _cbrRateService.GetRatesForPeriod(first, second, code);
            if (!Equals(result, _emptyPeriodRateList))
            {
                _cache.Set(
                    (first, second),
                    result,
                    new MemoryCacheEntryOptions
                    {
                        AbsoluteExpirationRelativeToNow = TimeSpan.FromDays(1)
                    });
            }

            return result;
        }
    }
}