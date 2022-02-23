using System.Threading;
using System.Threading.Tasks;
using MediatR;
using RateGetters.Contexts;
using RateGetters.Rates.Models;

namespace DomainServices.Commands.Rates.InsertRateForDate;

public class InsertRateForDateHandler : IRequestHandler<InsertRateForDateCommand, int>
{
    private readonly IContext _context;

    public InsertRateForDateHandler(IContext context) => _context = context;
        
    public async Task<int> Handle(InsertRateForDateCommand request, CancellationToken cancellationToken) =>
        await _context.InsertRateForDate(
                new RateForDate(
                    request.Specification.Code,
                    request.Specification.Value,
                    request.Specification.Date))
            .ConfigureAwait(false);
}
