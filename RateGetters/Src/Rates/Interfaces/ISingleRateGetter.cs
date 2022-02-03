using System;
using RateGetters.Rates.Getters;

namespace RateGetters.Rates.Interfaces
{
    public interface ISingleRateGetter
    {
        public RateGetterResult<RateForDate> GetRate(DateTime dateTime, CurrencyCodesEnum code);
    }
}