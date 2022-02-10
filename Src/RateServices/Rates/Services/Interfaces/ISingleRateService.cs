using System;
using System.Threading.Tasks;
using RateGetters.Rates.Models;
using RateGetters.Rates.Models.Enums;

namespace RateGetters.Rates.Services.Interfaces
{
    public interface ISingleRateService
    {
        public Task<RateForDate> GetRateAsync(DateTime dateTime, CurrencyCodesEnum code);
    }
}