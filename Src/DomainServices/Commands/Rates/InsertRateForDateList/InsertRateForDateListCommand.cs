using System.Collections.Generic;
using MediatR;
using Mongo.Models;

namespace DomainServices.Commands.Rates.InsertRateForDateList;

public record InsertRateForDateListSpecification(IEnumerable<RateForDate> List);
public record InsertRateForDateListCommand(InsertRateForDateListSpecification Specification) : IRequest<int>;
