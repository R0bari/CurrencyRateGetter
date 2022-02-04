using Xunit;
using System;
using System.Threading.Tasks;
using RateGetters.Rates.Models;
using RateGetters.Rates.Models.Enums;
using RateGetters.Rates.Services;
using WebAPI.Endpoints.Rates.GetForDate;
using WebAPI.Endpoints.Rates.GetForDateEndpoint;
using WebAPI.Endpoints.Rates.GetForPeriod;
using WebAPI.Endpoints.Rates.GetForPeriodEndpoint;

namespace WebAPI.Tests.Rates
{
    public class RateTests
    {
        private readonly GetForDateEndpoint _getForDateEndpoint;
        private readonly GetForPeriodEndpoint _getForPeriodEndpoint;

        public RateTests()
        {
            var rateGetter = new CbrRateService();
            _getForDateEndpoint = new GetForDateEndpoint(rateGetter);
            _getForPeriodEndpoint = new GetForPeriodEndpoint(rateGetter);
        }

        [Fact]
        public async Task TestGetByDate()
        {
            var date = new DateTime(2022, 02, 04);
            var result = await _getForDateEndpoint
                .HandleAsync(new GetForDateRequest(date, CurrencyCodesEnum.Eur))
                .ConfigureAwait(false);

            Assert.Equal(
                new RateForDate(
                    new Rate(CurrencyCodesEnum.Eur, 86.561m),
                    date),
                result.Value);
        }

        [Fact]
        public async Task TestGetByPeriod()
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

            var result = await _getForPeriodEndpoint
                .HandleAsync(new GetForPeriodRequest(CurrencyCodesEnum.Usd, first, second))
                .ConfigureAwait(false);

            Assert.Equal(expectedResult, result.Value);
        }
    }
}