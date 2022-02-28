using System;
using Domain.Models.Rates.Enums;
using MediatR;

namespace DomainServices.Commands.Rates.InsertRateForDate;

public record InsertRateForDateSpecification(CurrencyCodesEnum Code, decimal Value, DateTime Date);

public record InsertRateForDateCommand(InsertRateForDateSpecification Specification) : IRequest<int>;
