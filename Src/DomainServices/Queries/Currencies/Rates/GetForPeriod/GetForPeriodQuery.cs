using System;
using Domain.Models.Rates;
using Domain.Models.Rates.Enums;
using MediatR;

namespace DomainServices.Queries.Currencies.Rates.GetForPeriod
{
    public record GetForPeriodSpecification(CurrencyCodesEnum Code, DateTime FirstDate, DateTime SecondDate);
    public record GetForPeriodQuery(GetForPeriodSpecification Specification) : IRequest<PeriodRateList>;
}
