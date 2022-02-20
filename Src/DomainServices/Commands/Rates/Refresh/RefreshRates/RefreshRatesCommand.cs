using MediatR;

namespace DomainServices.Commands.Rates.Refresh.RefreshRates;

public record RefreshRatesCommand : IRequest<int>;
