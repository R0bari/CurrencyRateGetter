using System;
using RateGetters.Rates.Getters;
using RateGetters.Rates.Models;
using RateGetters.Rates.Models.Enums;

namespace RateGetters.Rates.Interfaces
{
    public interface IPeriodRateGetter
    {
        public PeriodRateList GetRatesForPeriod(DateTime first, DateTime second, CurrencyCodesEnum code);
    }
}