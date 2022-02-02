using System;
using RateGetters.Currencies;
using RateGetters.Currencies.Getters;

namespace CurrencyView
{
    internal static class Program
    {
        private static void Main(string[] args)
        {
            IRateGetter rateGetter = new CbrRateGetter();

            var usdRateResultForToday = rateGetter.GetRate(DateTime.Now, CurrencyCodesEnum.Usd);
            var usdRateResultForJuly = rateGetter.GetRate(new DateTime(1021, 06, 1), CurrencyCodesEnum.Usd);
            var usdRateResultForTomorrow = rateGetter.GetRate(DateTime.Now.AddDays(1), CurrencyCodesEnum.Usd);
            
            Console.WriteLine(usdRateResultForToday.IsSuccess
                ? usdRateResultForToday.RateForDate
                : usdRateResultForToday.ErrorMessage);
            Console.WriteLine(usdRateResultForJuly.IsSuccess
                ? usdRateResultForJuly.RateForDate
                : usdRateResultForJuly.ErrorMessage);
            Console.WriteLine(usdRateResultForTomorrow.IsSuccess
                ? usdRateResultForTomorrow.RateForDate
                : usdRateResultForTomorrow.ErrorMessage);
        }
    }
}