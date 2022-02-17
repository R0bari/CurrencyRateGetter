using System.Threading;
using System.Threading.Tasks;
using MediatR;
using RateGetters.Rates.Models;
using RateGetters.Rates.Services.Interfaces;

namespace CommandLayer.Queries.Currencies.Rates.GetForPeriodQuery
{
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
}