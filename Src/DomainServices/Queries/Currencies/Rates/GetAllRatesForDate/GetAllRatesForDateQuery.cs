using System;
using System.Collections.Generic;
using Domain.Models.Rates;
using MediatR;

namespace DomainServices.Queries.Currencies.Rates.GetAllRatesForDate;

public record GetAllRatesForDateSpecification(DateTime DateTime);
public record GetAllRatesForDateQuery(GetAllRatesForDateSpecification Specification) : IRequest<List<RateForDate>>;
