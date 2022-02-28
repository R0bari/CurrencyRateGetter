using System;
using System.Threading.Tasks;
using Domain.Models.Rates;
using Domain.Models.Rates.Enums;

namespace DomainServices.Services.Rates.Interfaces;

public interface ISingleRateService
{
    public Task<RateForDate> GetRateAsync(DateTime date, CurrencyCodesEnum code);
}
