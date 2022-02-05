using System.Threading;
using System.Threading.Tasks;
using MediatR;
using RateGetters.Rates.Models;
using RateGetters.Rates.Services.Interfaces;

namespace CommandLayer.Queries.Rates.GetForDateQuery
{
    public class GetForDateHandler : IRequestHandler<GetForDateQuery, RateForDate>
    {
        private readonly IRateService _rateService;

        public GetForDateHandler(IRateService rateService) => _rateService = rateService;
        
        public async Task<RateForDate> Handle(GetForDateQuery request, CancellationToken cancellationToken) =>
            await Task
                .Run(
                    () => _rateService.GetRate(request.Specification.DateTime, request.Specification.Code),
                    cancellationToken)
                .ConfigureAwait(false);
    }
}