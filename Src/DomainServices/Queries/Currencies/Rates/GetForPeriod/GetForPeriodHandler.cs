using System.Threading;
using System.Threading.Tasks;
using Domain.Models.Rates;
using DomainServices.Services.Rates.Interfaces;
using MediatR;

namespace DomainServices.Queries.Currencies.Rates.GetForPeriod;

public class GetForPeriodHandler : IRequestHandler<GetForPeriodQuery, PeriodRateList>
{
    private readonly IRateService _rateService;

    public GetForPeriodHandler(IRateService rateService) => _rateService = rateService;

    public async Task<PeriodRateList> Handle(GetForPeriodQuery request, CancellationToken cancellationToken) =>
        await _rateService.GetRatesForPeriodAsync(
                request.Specification.FirstDate,
                request.Specification.SecondDate,
                request.Specification.Code)
            .ConfigureAwait(false);
}
