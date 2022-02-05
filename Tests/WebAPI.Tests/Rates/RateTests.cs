using Xunit;
using Moq;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;
using CommandLayer.Queries.Rates.GetForDateQuery;
using CommandLayer.Queries.Rates.GetForPeriodQuery;
using RateGetters.Rates.Models;
using RateGetters.Rates.Models.Enums;
using RateGetters.Rates.Services;
using RateGetters.Rates.Services.Interfaces;
using WebAPI.Endpoints.Rates.GetForDate;
using WebAPI.Endpoints.Rates.GetForPeriod;

namespace WebAPI.Tests.Rates
{
    public class RateTests
    {
        private readonly GetForDateEndpoint _getForDateEndpoint;
        private readonly GetForPeriodEndpoint _getForPeriodEndpoint;

        public RateTests()
        {
            var mediatorMock = PrepareMediatorMock(new CbrRateService());
            _getForDateEndpoint = new GetForDateEndpoint(mediatorMock.Object);
            _getForPeriodEndpoint = new GetForPeriodEndpoint(mediatorMock.Object);
        }

        [Fact]
        public async Task TestGetForDate()
        {
            var date = new DateTime(2022, 02, 04);
            var result = await _getForDateEndpoint
                .HandleAsync(new GetForDateSpecification(date, CurrencyCodesEnum.Eur))
                .ConfigureAwait(false);

            Assert.Equal(
                new RateForDate(
                    new Rate(CurrencyCodesEnum.Eur, 86.561m),
                    date),
                result);
        }

        [Fact]
        public async Task TestGetForPeriod()
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
                .HandleAsync(new GetForPeriodSpecification(CurrencyCodesEnum.Usd, first, second))
                .ConfigureAwait(false);

            Assert.Equal(expectedResult, result);
        }

        private static Mock<IMediator> PrepareMediatorMock(IRateService rateService)
        {
            var mediator = new Mock<IMediator>();
            mediator
                .Setup(m => m.Send(
                    It.IsAny<GetForDateQuery>(),
                    It.IsAny<CancellationToken>()))
                .Returns((GetForDateQuery query, CancellationToken token) =>
                    Task.FromResult(
                        rateService.GetRate(
                            query.Specification.DateTime,
                            query.Specification.Code)));
            mediator
                .Setup(m => m.Send(
                    It.IsAny<GetForPeriodQuery>(),
                    It.IsAny<CancellationToken>()))
                .Returns((GetForPeriodQuery query, CancellationToken token) =>
                    Task.FromResult(
                        rateService.GetRatesForPeriod(
                            query.Specification.FirstDate,
                            query.Specification.SecondDate,
                            query.Specification.Code)));
            
            return mediator;
        }
    }
}