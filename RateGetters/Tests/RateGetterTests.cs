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

        [Fact]
        public void TestSuccessfulCbrRateGetterSingleResult()
        {
            var now = DateTime.Now;
            var rateResultForToday = _rateGetter.GetRate(now, CurrencyCodesEnum.Usd);
            AssertStatusSuccess(rateResultForToday);
            Assert.Equal(now, rateResultForToday.Result.DateTime);
            Assert.Equal(CurrencyCodesEnum.Usd, rateResultForToday.Result.Rate.Code);
        }

        [Fact]
        public void TestFailedCbrRateGetterSingleResult()
        {
            var farPast = new DateTime(1021, 06, 1);
            AssertStatusFail(_rateGetter.GetRate(farPast, CurrencyCodesEnum.Usd));
        }

        [Fact]
        public void TestSuccessfulCbrRateGetterPeriodResult()
        {
            var periodStart = new DateTime(2021, 12, 01);
            var holidayDate = new DateTime(2022, 01, 04);
            var firstBeforeHolidayDate = new DateTime(2021, 12, 31);
            var periodRateResult = _rateGetter.GetRateForPeriod(
                holidayDate,
                periodStart,
                CurrencyCodesEnum.Usd);
            AssertStatusSuccess(periodRateResult);
            Assert.Equal(23, periodRateResult.Result.Count());
            Assert.Equal(periodStart, periodRateResult.Result.First().DateTime);
            Assert.Equal(firstBeforeHolidayDate, periodRateResult.Result.Last().DateTime);
        }

        [Fact]
        public void TestFailedCbrRateGetterPeriodResult()
        {
            var start = new DateTime(1021, 06, 1);
            var end = DateTime.Now;
            var periodRateResult = _rateGetter.GetRateForPeriod(start, end, CurrencyCodesEnum.Usd);
            AssertStatusFail(periodRateResult);
        }


        private static void AssertStatusSuccess<T>(RateGetterResult<T> result)
        {
            Assert.Equal(string.Empty, result.ErrorMessage);
            Assert.True(result.IsSuccess);
        }

        private void AssertStatusFail<T>(RateGetterResult<T> result)
        {
            var expectedErrorMessage = GetPropValue(_rateGetter, "CurrencyNotFoundErrorMessage").ToString();
            Assert.False(result.IsSuccess);
            Assert.Equal(expectedErrorMessage, result.ErrorMessage);
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