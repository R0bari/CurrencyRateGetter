using System;
using MediatR;

namespace DomainServices.Queries.Currencies.Rates.GetMostRecentDate;

public record GetMostRecentDateQuery : IRequest<DateTime>;
