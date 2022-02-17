using System;
using MediatR;
using RateGetters.Rates.Models;
using RateGetters.Rates.Models.Enums;

namespace CommandLayer.Queries.Currencies.Rates.GetForPeriodQuery
{
    public record GetForPeriodSpecification(CurrencyCodesEnum Code, DateTime FirstDate, DateTime SecondDate);
    public record GetForPeriodQuery(GetForPeriodSpecification Specification) : IRequest<PeriodRateList>;
}