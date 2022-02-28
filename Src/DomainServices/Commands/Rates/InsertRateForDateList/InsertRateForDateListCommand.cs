using System.Collections.Generic;
using Domain.Models.Rates;
using MediatR;

namespace DomainServices.Commands.Rates.InsertRateForDateList;

public record InsertRateForDateListSpecification(IEnumerable<RateForDate> List);
public record InsertRateForDateListCommand(InsertRateForDateListSpecification Specification) : IRequest<int>;
