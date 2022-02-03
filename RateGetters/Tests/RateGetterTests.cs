using System;
using System.Linq;
using System.Reflection;
using RateGetters.Rates.Getters;
using RateGetters.Rates.Interfaces;
using RateGetters.Rates.Models;
using Xunit;

namespace RateGetters.Tests
{
    public class RateGetterTests
    {
        private readonly IRateGetter _rateGetter;

        public RateGetterTests()
        {
            _rateGetter = new CbrRateGetter();
        }

        [Theory]
        [InlineData(76.4849, CurrencyCodesEnum.Usd, 2022, 02, 03)]
        [InlineData(86.2826, CurrencyCodesEnum.Eur, 2022, 02, 03)]
        public void TestCurrenciesRates(decimal expectedValue, CurrencyCodesEnum code, int year, int month, int day)
        {
            var date = new DateTime(year, month, day);
            Assert.Equal(
                new RateForDate(
                    new Rate(code, expectedValue),
                    date),
                _rateGetter
                    .GetRate(date, code).Result);
        }
        
        [Fact]
        public void TestFailedCbrRateGetterSingleResult()
        {
            var farPast = new DateTime(1021, 06, 1);
            Assert.Equal(
                OperationResult<RateForDate>.Failed("Given rate for given currency not found."),
                _rateGetter.GetRate(farPast, CurrencyCodesEnum.Eur));
        }

        [Fact]
        public void TestSuccessfulCbrRateGetterPeriodResult()
        {
            var periodStart = new DateTime(2021, 12, 01);
            var holidayDate = new DateTime(2022, 01, 04);
            var firstBeforeHolidayDate = new DateTime(2021, 12, 31);
            var periodRateResult = _rateGetter.GetRatesForPeriod(
                holidayDate,
                periodStart,
                CurrencyCodesEnum.Usd);
            Assert.Equal(23, periodRateResult.Result.Count());
            Assert.Equal(
                new RateForDate(
                    new Rate(CurrencyCodesEnum.Usd, 74.8926m),
                    periodStart),
                periodRateResult.Result.First());
            Assert.Equal(
                new RateForDate(
                    new Rate(CurrencyCodesEnum.Usd, 74.2926m),
                    firstBeforeHolidayDate),
                periodRateResult.Result.Last());
        }
    }
}