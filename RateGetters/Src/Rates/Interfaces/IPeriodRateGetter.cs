using System;
using System.Collections.Generic;
using RateGetters.Rates.Getters;

namespace RateGetters.Rates.Interfaces
{
    public interface IPeriodRateGetter
    {
        public RateGetterResult<IEnumerable<RateForDate>> GetRateForPeriod(
            DateTime first,
            DateTime second,
            CurrencyCodesEnum code);
    }
}