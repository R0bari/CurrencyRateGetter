using System;
using Domain.Models.Rates;
using Domain.Models.Rates.Enums;
using MediatR;

namespace DomainServices.Queries.Currencies.Rates.GetForDate;

public record GetForDateSpecification(CurrencyCodesEnum Code, DateTime Date);
public record GetForDateQuery(GetForDateSpecification Specification) : IRequest<RateForDate>;
