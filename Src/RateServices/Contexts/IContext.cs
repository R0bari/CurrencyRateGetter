using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using RateGetters.Rates.Models;
using RateGetters.Rates.Models.Enums;

namespace RateGetters.Contexts;

public interface IContext
{
    Task<RateForDate> GetRateForDate(CurrencyCodesEnum code, DateTime date);
    
    Task<DateTime> GetMostRecentDate();
    
    Task<int> InsertRateForDate(RateForDate rateForDate);
    
    Task<int> InsertRateForDateList(IEnumerable<RateForDate> ratesForDate);
    
    Task<int> DeleteRate(RateForDate rate);

    Task<int> DeleteAllRates(bool confirm = false);
}
