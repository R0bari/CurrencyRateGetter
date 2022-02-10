using System;
using System.Threading.Tasks;
using RateGetters.Rates.Models;
using RateGetters.Rates.Models.Enums;

namespace RateGetters.Rates.Services.Interfaces
{
    public interface IPeriodRateService
    {
        public Task<PeriodRateList> GetRatesForPeriodAsync(DateTime first, DateTime second, CurrencyCodesEnum code);
    }
}