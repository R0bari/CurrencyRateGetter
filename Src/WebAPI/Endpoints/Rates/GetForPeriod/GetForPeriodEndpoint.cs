using System.Threading;
using System.Threading.Tasks;
using Ardalis.ApiEndpoints;
using Microsoft.AspNetCore.Mvc;
using RateGetters.Rates.Interfaces;
using RateGetters.Rates.Models;
using Swashbuckle.AspNetCore.Annotations;

namespace WebAPI.Endpoints.Rates.GetForPeriod
{
    public class GetForPeriodEndpoint : EndpointBaseAsync
        .WithRequest<GetForPeriodRequest>
        .WithResult<ActionResult<PeriodRateList>>
    {
        private readonly IRateService _rateService;

        public GetForPeriodEndpoint(IRateService rateService) => _rateService = rateService;

        [HttpGet("rates/period")]
        [SwaggerOperation(
            Summary = "Get currency rate for period",
            Description = "Get currency rate for period",
            OperationId = "Rate.GetForPeriod",
            Tags = new[] {"RateEndpoint"})]
        public override async Task<ActionResult<PeriodRateList>> HandleAsync(
            [FromQuery] GetForPeriodRequest request,
            CancellationToken cancellationToken = default)
        {
            return await Task.Run(
                    () => new ActionResult<PeriodRateList>(
                        _rateService.GetRatesForPeriod(request.FirstDate, request.SecondDate, request.Code)),
                    cancellationToken)
                .ConfigureAwait(false);
        }
    }
}