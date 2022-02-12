using System;
using MediatR;
using RateGetters.Rates.Models;
using RateGetters.Rates.Models.Enums;

namespace CommandLayer.Queries.Rates.GetForDateQuery
{
    public record GetForDateSpecification(CurrencyCodesEnum Code, DateTime DateTime);
    public record GetForDateQuery(GetForDateSpecification Specification) : IRequest<RateForDate>;
}