using System;
using System.Threading;
using System.Threading.Tasks;
using Domain.Contexts;
using MediatR;

namespace DomainServices.Queries.Currencies.Rates.GetMostRecentDate;

public class GetMostRecentDateHandler : IRequestHandler<GetMostRecentDateQuery, DateTime>
{
    private readonly IContext _context;

    public GetMostRecentDateHandler(IContext context)
    {
        _context = context;
    }

    public async Task<DateTime> Handle(GetMostRecentDateQuery request, CancellationToken cancellationToken) =>
        await _context
            .GetMostRecentDate()
            .ConfigureAwait(false);
}
