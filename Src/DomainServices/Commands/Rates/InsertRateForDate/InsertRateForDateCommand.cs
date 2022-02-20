using System;
using MediatR;
using RateGetters.Rates.Models.Enums;

namespace DomainServices.Commands.Rates.InsertRateForDate;

public record InsertRateForDateSpecification(CurrencyCodesEnum Code, decimal Value, DateTime Date);

public record InsertRateForDateCommand(InsertRateForDateSpecification Specification) : IRequest<int>;
