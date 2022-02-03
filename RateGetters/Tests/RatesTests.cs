using System;
using RateGetters.Rates;
using RateGetters.Rates.Getters;
using RateGetters.Rates.Interfaces;
using RateGetters.Rates.Models;
using Xunit;

namespace RateGetters.Tests
{
    public class RatesTests
    {
        private readonly IRateGetter _rateGetter;
        public RatesTests()
        {
            _rateGetter = new CbrRateGetter();
        }

        [Theory]
        [InlineData(76.4849, CurrencyCodesEnum.Usd, 2022, 02, 03)]
        [InlineData(86.2826, CurrencyCodesEnum.Eur, 2022, 02, 03)]
        public void TestCurrenciesRates(decimal expectedValue, CurrencyCodesEnum code, int year, int month, int day)
        {
            Assert.Equal(
                expectedValue,
                _rateGetter
                    .GetRate(new DateTime(year,month, day), code)
                    .Result
                    .Rate
                    .Value);
        }
    }
}