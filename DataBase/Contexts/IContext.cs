using Mongo.Models;
using RateGetters.Rates.Models.Enums;

namespace Mongo.Contexts;

public interface IContext
{
    Task<RateForDate> GetRateForDate(CurrencyCodesEnum code, DateTime dateTime);
    Task<int> InsertRateForDate(RateForDate rateForDate);

    Task<int> DeleteRateForDate(string id);
}
