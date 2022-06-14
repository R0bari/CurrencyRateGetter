using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Domain.Models.Rates;
using Domain.Models.Rates.Enums;

namespace Domain.Contexts;

public interface IContext
{
    Task<RateForDate> GetRateForDate(CurrencyCodesEnum code, DateTime date);
    
    Task<RateForDateList> GetAllRatesForDate(DateTime dateTime);
    
    Task<int> InsertRateForDate(RateForDate rateForDate);
    
    Task<int> InsertRateForDateList(IEnumerable<RateForDate> ratesForDate);
    
    Task<int> DeleteRate(RateForDate rate);

    Task<int> DeleteAllRates(bool confirm = false);
}
