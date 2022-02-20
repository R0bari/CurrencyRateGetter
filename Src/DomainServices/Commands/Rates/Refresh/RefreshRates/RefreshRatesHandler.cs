using System;
using System.Threading;
using System.Threading.Tasks;
using DomainServices.Commands.Rates.Refresh.RefreshFromDate;
using DomainServices.Commands.Rates.RemoveAllRates;
using MediatR;

namespace DomainServices.Commands.Rates.Refresh.RefreshRates;

public class RefreshRatesHandler : IRequestHandler<RefreshRatesCommand, int>
{
    private readonly IMediator _mediator;

    public RefreshRatesHandler(IMediator mediator)
    {
        _mediator = mediator;
    }

    public async Task<int> Handle(RefreshRatesCommand request, CancellationToken cancellationToken)
    {
        await _mediator.Send(
                new RemoveAllRatesCommand(),
                cancellationToken)
            .ConfigureAwait(false);
        return await _mediator.Send(
                new RefreshRatesFromDateCommand(
                    new DateTime(2000, 01, 01)),
                cancellationToken)
            .ConfigureAwait(false);
    }
}
