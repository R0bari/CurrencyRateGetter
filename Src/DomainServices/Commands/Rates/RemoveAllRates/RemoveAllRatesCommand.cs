using MediatR;

namespace DomainServices.Commands.Rates.RemoveAllRates;

public record RemoveAllRatesCommand() : IRequest<int>;
