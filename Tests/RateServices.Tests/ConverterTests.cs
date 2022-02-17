using System;
using Microsoft.Extensions.Caching.Memory;
using RateGetters.Converters;
using RateGetters.Rates.Models.Enums;
using RateGetters.Rates.Services;
using Xunit;

namespace RateGetters.Tests
{
    public class ConverterTests
    {
        private readonly IConverter _converter;

        public ConverterTests() =>
            _converter =
                new BaseConverter(
                    new CachedCbrRateService(
                        new MemoryCache(
                            new MemoryCacheOptions())));

        [Theory]
        [InlineData(1.13719953982, CurrencyCodesEnum.Eur, CurrencyCodesEnum.Usd, 1)]
        [InlineData(75.0141, CurrencyCodesEnum.Usd, CurrencyCodesEnum.Rub, 1)]
        [InlineData(1533.04512085061, CurrencyCodesEnum.Rub, CurrencyCodesEnum.Usd, 115_000)]
        [InlineData(2266.24061343134, CurrencyCodesEnum.Rub, CurrencyCodesEnum.Usd, 170_000)]
        public async void TestConversations(
            decimal expectedValue,
            CurrencyCodesEnum from,
            CurrencyCodesEnum to,
            decimal actualValue)
        {
            var actual = await _converter.Convert(from, to, actualValue); 
            Assert.Equal(expectedValue, Math.Round(actual, 11));
        }
    }
}