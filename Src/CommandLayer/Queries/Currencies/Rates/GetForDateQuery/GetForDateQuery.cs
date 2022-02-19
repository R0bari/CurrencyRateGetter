using System;
using MediatR;
using RateGetters.Rates.Models;
using RateGetters.Rates.Models.Enums;

namespace CommandLayer.Queries.Currencies.Rates.GetForDateQuery
{
    public record GetForDateSpecification(CurrencyCodesEnum Code, DateTime Date);
    public record GetForDateQuery(GetForDateSpecification Specification) : IRequest<RateForDate>;
}
