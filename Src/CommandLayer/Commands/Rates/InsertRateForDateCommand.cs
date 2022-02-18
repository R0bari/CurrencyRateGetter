using System;
using MediatR;
using RateGetters.Rates.Models.Enums;

namespace CommandLayer.Commands.Rates
{
    public record InsertRateForDateSpecification(CurrencyCodesEnum Code, decimal Value, DateTime DateTime);

    public record InsertRateForDateCommand(InsertRateForDateSpecification Specification) : IRequest<int>;
}
