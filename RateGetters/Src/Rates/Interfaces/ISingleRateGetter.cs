using System;
using RateGetters.Rates.Getters;
using RateGetters.Rates.Models;

namespace RateGetters.Rates.Interfaces
{
    public interface ISingleRateGetter
    {
        public OperationResult<RateForDate> GetRate(DateTime dateTime, CurrencyCodesEnum code);
    }
}