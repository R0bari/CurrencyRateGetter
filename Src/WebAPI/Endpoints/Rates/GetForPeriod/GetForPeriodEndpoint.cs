using System.Threading;
using System.Threading.Tasks;
using Ardalis.ApiEndpoints;
using Microsoft.AspNetCore.Mvc;
using RateGetters.Rates.Getters;
using RateGetters.Rates.Interfaces;
using RateGetters.Rates.Models;
using Swashbuckle.AspNetCore.Annotations;
using WebAPI.Endpoints.Rates.GetForPeriodEndpoint;

namespace WebAPI.Endpoints.Rates.GetForPeriod
{
    public class GetForPeriodEndpoint : EndpointBaseAsync
        .WithRequest<GetForPeriodRequest>
        .WithResult<ActionResult<OperationResult<PeriodRateList>>>
    {
        private readonly IRateGetter _rateGetter;

        public GetForPeriodEndpoint(IRateGetter rateGetter) => _rateGetter = rateGetter;

        [HttpGet("rates/period")]
        [SwaggerOperation(
            Summary = "Get currency rate for period",
            Description = "Get currency rate for period",
            OperationId = "Rate.GetForPeriod",
            Tags = new[] {"RateEndpoint"})]
        public override async Task<ActionResult<OperationResult<PeriodRateList>>> HandleAsync(
            [FromQuery] GetForPeriodRequest request,
            CancellationToken cancellationToken = default)
        {
            return await Task.Run(
                    () => new ActionResult<OperationResult<PeriodRateList>>(
                        _rateGetter.GetRatesForPeriod(request.FirstDate, request.SecondDate, request.Code)),
                    cancellationToken)
                .ConfigureAwait(false);
        }
    }
}