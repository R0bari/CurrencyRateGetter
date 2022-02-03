using System;
using RateGetters.Rates.Getters;
using RateGetters.Rates.Models;

namespace RateGetters.Rates.Interfaces
{
    public interface IPeriodRateGetter
    {
        public OperationResult<PeriodRateList> GetRatesForPeriod(
            DateTime first,
            DateTime second,
            CurrencyCodesEnum code);
    }
}