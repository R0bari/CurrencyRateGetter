using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Mongo.Contexts;

namespace DomainServices.Commands.Rates.RemoveAllRates;

public class RemoveAllRatesHandler : IRequestHandler<RemoveAllRatesCommand, int>
{
    private readonly IContext _context;

    public RemoveAllRatesHandler(IContext context)
    {
        _context = context;
    }

    public async Task<int> Handle(RemoveAllRatesCommand request, CancellationToken cancellationToken) =>
        await _context
            .DeleteAllRates(true)
            .ConfigureAwait(false);
}
