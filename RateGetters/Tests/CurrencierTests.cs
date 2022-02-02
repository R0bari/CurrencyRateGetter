using System;
using System.Reflection;
using RateGetters.Currencies;
using RateGetters.Currencies.Getters;
using Xunit;

namespace RateGetters.Tests
{
    public class CurrencierTests
    {
        [Fact]
        public void TestSuccessfulCbrRateGetterResult()
        {
            IRateGetter rateGetter = new CbrRateGetter();
            var now = DateTime.Now;
            var usdRateResultForToday = rateGetter.GetRate(now, CurrencyCodesEnum.Usd);
            Assert.Equal(true, usdRateResultForToday.IsSuccess);
            Assert.Equal(string.Empty, usdRateResultForToday.ErrorMessage);
            Assert.Equal(now, GetPropValue(usdRateResultForToday.RateForDate, "DateTime"));
            Assert.Equal(CurrencyCodesEnum.Usd, GetPropValue(usdRateResultForToday.RateForDate, "Code"));
        }
        
        [Fact]
        public void TestFailedCbrRateGetterResult()
        {
            IRateGetter rateGetter = new CbrRateGetter();
            var farPast = new DateTime(1021, 06, 1);
            const string expectedErrorMessage = "Given rate for given currency not found.";
            var usdRateResultForToday = rateGetter.GetRate(farPast, CurrencyCodesEnum.Usd);
            Assert.Equal(false,usdRateResultForToday.IsSuccess);
            Assert.Equal(expectedErrorMessage, usdRateResultForToday.ErrorMessage);
        }
        
        private static object GetPropValue(object src, string propName)
        {
            return src
                .GetType()
                .GetProperty(propName, BindingFlags.NonPublic | BindingFlags.Instance)
                ?.GetGetMethod(true)
                ?.Invoke(src, null);
        }
    }
}