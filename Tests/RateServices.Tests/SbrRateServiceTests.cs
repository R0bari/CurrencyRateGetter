using Xunit;
using System;
using RateGetters.Rates.Models;
using RateGetters.Rates.Models.Enums;
using RateGetters.Rates.Services;
using RateGetters.Rates.Services.Interfaces;
using Microsoft.Extensions.Caching.Memory;

namespace RateGetters.Tests
{
    public class SbrRateServiceTests
    {
        private readonly IRateService _rateService;

        public SbrRateServiceTests() =>
            _rateService =
                new CachedCbrRateService(
                    new MemoryCache(
                        new MemoryCacheOptions()));

        [Theory]
        [InlineData(76.4849, CurrencyCodesEnum.Usd, 2022, 02, 03)]
        [InlineData(86.2826, CurrencyCodesEnum.Eur, 2022, 02, 03)]
        public void TestCurrenciesRates(decimal expectedValue, CurrencyCodesEnum code, int year, int month, int day) =>
            Assert.Equal(
                new RateForDate(
                    new Rate(code, expectedValue),
                    new DateTime(year, month, day)),
                _rateService.GetRate(
                    new DateTime(year, month, day),
                    code));

        [Fact]
        public void TestFailedCbrRateGetterSingleResult() =>
            Assert.Equal(
                new RateForDate(
                    new Rate(CurrencyCodesEnum.None, 0m),
                    DateTime.MinValue),
                _rateService.GetRate(
                    new DateTime(1021, 06, 1),
                    CurrencyCodesEnum.Eur));

        [Fact]
        public void TestSuccessfulCbrRateGetterPeriodResult() =>
            Assert.Equal(
                new PeriodRateList(
                    new RateForDate[]
                    {
                        new(
                            new Rate(CurrencyCodesEnum.Usd, 74.8926m),
                            new DateTime(2021, 12, 01)),
                        new(
                            new Rate(CurrencyCodesEnum.Usd, 73.9746m),
                            new DateTime(2021, 12, 02)),
                        new(
                            new Rate(CurrencyCodesEnum.Usd, 74.0637m),
                            new DateTime(2021, 12, 03)),
                        new(
                            new Rate(CurrencyCodesEnum.Usd, 73.7426m),
                            new DateTime(2021, 12, 04))
                    }),
                _rateService.GetRatesForPeriod(
                    new DateTime(2021, 12, 01),
                    new DateTime(2021, 12, 06),
                    CurrencyCodesEnum.Usd));

        [Fact]
        public void TestSuccessfulCbrRateGetterPeriodResultWithHolidays() =>
            Assert.Equal(
                new PeriodRateList(
                    new RateForDate[]
                    {
                        new(
                            new Rate(CurrencyCodesEnum.Usd, 74.8926m),
                            new DateTime(2021, 12, 01)),
                        new(
                            new Rate(CurrencyCodesEnum.Usd, 73.9746m),
                            new DateTime(2021, 12, 02)),
                        new(
                            new Rate(CurrencyCodesEnum.Usd, 74.0637m),
                            new DateTime(2021, 12, 03)),
                        new(
                            new Rate(CurrencyCodesEnum.Usd, 73.7426m),
                            new DateTime(2021, 12, 04))
                    }),
                _rateService.GetRatesForPeriod(
                    new DateTime(2021, 12, 01),
                    new DateTime(2021, 12, 06),
                    CurrencyCodesEnum.Usd));
    }
}