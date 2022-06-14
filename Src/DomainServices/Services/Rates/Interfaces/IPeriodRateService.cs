using System;
using System.Threading.Tasks;
using Domain.Models.Rates;
using Domain.Models.Rates.Enums;

namespace DomainServices.Services.Rates.Interfaces
{
    public interface IPeriodRateService
    {
        public Task<RateForDateList> GetRatesForPeriodAsync(DateTime first, DateTime second, CurrencyCodesEnum code);
    }
}