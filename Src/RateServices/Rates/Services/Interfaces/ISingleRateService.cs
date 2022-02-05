using System;
using RateGetters.Rates.Models;
using RateGetters.Rates.Models.Enums;

namespace RateGetters.Rates.Services.Interfaces
{
    public interface ISingleRateService
    {
        public RateForDate GetRate(DateTime dateTime, CurrencyCodesEnum code);
    }
}