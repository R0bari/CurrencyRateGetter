using System;
using RateGetters.Rates.Models;
using RateGetters.Rates.Models.Enums;

namespace RateGetters.Rates.Services.Interfaces
{
    public interface IPeriodRateService
    {
        public PeriodRateList GetRatesForPeriod(DateTime first, DateTime second, CurrencyCodesEnum code);
    }
}