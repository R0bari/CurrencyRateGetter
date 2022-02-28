using System.Threading;
using System.Threading.Tasks;
using Domain.Contexts;
using Domain.Models.Rates;
using DomainServices.Services.Rates.Interfaces;
using MediatR;

namespace DomainServices.Queries.Currencies.Rates.GetForDate;

public class GetForDateHandler : IRequestHandler<GetForDateQuery, RateForDate>
{
    private readonly IRateService _rateService;
    private readonly IContext _context;

    public GetForDateHandler(IRateService rateService, IContext context)
    {
        _rateService = rateService;
        _context = context;
    }

    public async Task<RateForDate> Handle(GetForDateQuery request, CancellationToken cancellationToken)
    {
        var contextResult = await _context.GetRateForDate(
                request.Specification.Code,
                request.Specification.Date)
            .ConfigureAwait(false);
        if (contextResult == RateForDate.Empty)
        {
            return await _rateService.GetRateAsync(
                    request.Specification.Date,
                    request.Specification.Code)
                .ConfigureAwait(false);
        }

        return contextResult;
    }
}