using System;
using System.Collections.Generic;
using RateGetters.Rates.Getters;
using RateGetters.Rates.Models;

namespace RateGetters.Rates.Interfaces
{
    public interface IPeriodRateGetter
    {
        public RateGetterResult<PeriodRateList> GetRateForPeriod(
            DateTime first,
            DateTime second,
            CurrencyCodesEnum code);
    }
}