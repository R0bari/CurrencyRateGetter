using Mongo.Models;
using RateGetters.Rates.Models.Enums;

namespace Mongo.Contexts;

public interface IContext
{
    Task<RateForDate> GetRateForDate(CurrencyCodesEnum code, DateTime date);
    
    Task<DateTime> GetMostRecentDate();
    
    Task<int> InsertRateForDate(RateForDate rateForDate);
    
    Task<int> InsertRatesForDate(IEnumerable<RateForDate> ratesForDate);
    
    Task<int> DeleteRateById(string id);

    Task<int> DeleteAllRates(bool confirm = false);
}