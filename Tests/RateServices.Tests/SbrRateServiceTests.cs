using Xunit;
using System;
using System.Linq;
using RateGetters.Rates.Interfaces;
using RateGetters.Rates.Models;
using RateGetters.Rates.Models.Enums;
using RateGetters.Rates.Services;

namespace RateGetters.Tests
{
    public class SbrRateServiceTests
    {
        private readonly IRateService _rateService;

        public SbrRateServiceTests()
        {
            _rateService = new CbrRateService();
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
                _rateService.GetRate(date, code));
        }

        [Fact]
        public void TestFailedCbrRateGetterSingleResult()
        {
            var farPast = new DateTime(1021, 06, 1);
            Assert.Equal(
                new RateForDate(
                    new Rate(CurrencyCodesEnum.None, 0m),
                    DateTime.MinValue),
                _rateService.GetRate(farPast, CurrencyCodesEnum.Eur));
        }

        [Fact]
        public void TestSuccessfulCbrRateGetterPeriodResult()
        {
            var first = new DateTime(2021, 12, 06);
            var second = new DateTime(2021, 12, 01);
            var expectedResult = new PeriodRateList(
                new RateForDate[]
                {
                    new(
                        new Rate(CurrencyCodesEnum.Usd, 74.8926m),
                        second),
                    new(
                        new Rate(CurrencyCodesEnum.Usd, 73.9746m),
                        second.AddDays(1)),
                    new(
                        new Rate(CurrencyCodesEnum.Usd, 74.0637m),
                        second.AddDays(2)),
                    new(
                        new Rate(CurrencyCodesEnum.Usd, 73.7426m),
                        second.AddDays(3))
                });

            var result = _rateService.GetRatesForPeriod(first, second, CurrencyCodesEnum.Usd);
            Assert.Equal(expectedResult, result);
        }

        [Fact]
        public void TestSuccessfulCbrRateGetterPeriodResultWithHolidays()
        {
            var periodStart = new DateTime(2021, 12, 01);
            var holidayDate = new DateTime(2022, 01, 04);
            var firstBeforeHolidayDate = new DateTime(2021, 12, 31);
            var periodRateResult = _rateService.GetRatesForPeriod(
                holidayDate,
                periodStart,
                CurrencyCodesEnum.Usd);
            Assert.Equal(23, periodRateResult.Count());
            Assert.Equal(
                new RateForDate(
                    new Rate(CurrencyCodesEnum.Usd, 74.8926m),
                    periodStart),
                periodRateResult.First());
            Assert.Equal(
                new RateForDate(
                    new Rate(CurrencyCodesEnum.Usd, 74.2926m),
                    firstBeforeHolidayDate),
                periodRateResult.Last());
        }
    }
}