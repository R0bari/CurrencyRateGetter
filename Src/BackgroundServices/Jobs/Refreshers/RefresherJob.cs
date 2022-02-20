using DomainServices.Commands.Rates.Refresh.RefreshRates;
using MediatR;
using Quartz;

namespace BackgroundServices.Jobs.Refreshers;

public class RefresherJob : IJob
{
    private readonly IMediator _mediator;
    private readonly ILogger<RefresherJob> _logger;

    public RefresherJob(ILogger<RefresherJob> logger, IMediator mediator)
    {
        _logger = logger;
        _mediator = mediator;
    }

    public async Task Execute(IJobExecutionContext context)
    {
        var result = await _mediator.Send(
                new RefreshRatesCommand())
            .ConfigureAwait(false);
        if (result >= 0)
        {
            _logger.Log(LogLevel.Information, "Refreshing rates completed");
            return;
        }
        _logger.Log(LogLevel.Error, "Refreshing rates failed");
    }
}
