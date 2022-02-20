using System;
using System.Threading;
using System.Threading.Tasks;
using Ardalis.ApiEndpoints;
using DomainServices.Queries.Currencies.Convert.ConvertCurrencyQuery;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace WebAPI.Endpoints.Rates;

public class ConvertCurrencyEndpoint : EndpointBaseAsync
    .WithRequest<ConvertCurrencySpecification>
    .WithResult<decimal>
{
    private readonly IMediator _mediator;

    public ConvertCurrencyEndpoint(IMediator mediator) =>
        _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
    
    [HttpGet("rates/convert")]
    [SwaggerOperation(
        Summary = "Convert currency",
        Description = "Convert one currency to another",
        OperationId = "Rate.Convert",
        Tags = new[] {"RateEndpoint"})]
    public override async Task<decimal> HandleAsync(
        [FromQuery] ConvertCurrencySpecification specification,
        CancellationToken cancellationToken = default) =>
        await _mediator
            .Send(new ConvertCurrencyQuery(specification), cancellationToken)
            .ConfigureAwait(false);
}
