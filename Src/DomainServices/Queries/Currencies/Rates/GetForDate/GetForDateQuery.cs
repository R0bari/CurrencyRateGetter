using System;
using MediatR;
using RateGetters.Rates.Models;
using RateGetters.Rates.Models.Enums;

namespace DomainServices.Queries.Currencies.Rates.GetForDate;

public record GetForDateSpecification(CurrencyCodesEnum Code, DateTime Date);
public record GetForDateQuery(GetForDateSpecification Specification) : IRequest<RateForDate>;
