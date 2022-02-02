using System;

namespace RateGetters.Currencies.Getters
{
    public interface IRateGetter
    {
        public RateGetterResult<RateForDate> GetRate(DateTime dateTime, CurrencyCodesEnum code);
        
        //  TODO: public RateGetterResult<List<RateForDate>> GetRateForPeriod(DateTime first, DateTime second, CurrencyCodesEnum code)
    }
}