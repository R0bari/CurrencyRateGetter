using System;
using RateGetters.Rates;
using RateGetters.Rates.Getters;
using RateGetters.Rates.Interfaces;
using RateGetters.Rates.Models;

namespace CurrencyView
{
    internal static class Program
    {
        private static void Main(string[] args)
        {
            IRateGetter rateGetter = new CbrRateGetter();
            
            var usdRateResultForToday = rateGetter.GetRate(DateTime.Now, CurrencyCodesEnum.Usd);
            var usdRateResultForJuly = rateGetter.GetRate(new DateTime(1021, 06, 1), CurrencyCodesEnum.Usd);
            var usdPeriodRateResult = rateGetter.GetRateForPeriod(
                new DateTime(2021, 12, 01),
                new DateTime(2022, 01, 04),
                CurrencyCodesEnum.Usd);
            
            Console.WriteLine(usdRateResultForToday.IsSuccess
                ? usdRateResultForToday.Result
                : usdRateResultForToday.ErrorMessage);
            Console.WriteLine(usdRateResultForJuly.IsSuccess
                ? usdRateResultForJuly.Result
                : usdRateResultForJuly.ErrorMessage);

            if (!usdPeriodRateResult.IsSuccess)
            {
                Console.WriteLine(usdPeriodRateResult.ErrorMessage);
                return;
            }

            foreach (var rateForDate in usdPeriodRateResult.Result)
            {
                Console.WriteLine(rateForDate.ToString());
            }
        }
    }
}