using System;
using MediatR;

namespace DomainServices.Commands.Rates.Refresh.RefreshFromDate;

public record RefreshRatesFromDateCommand(DateTime Date) : IRequest<int>;
