using System.Threading.Tasks;
using DomainServices.Commands.Rates.Refresh.RefreshRates;
using MediatR;
using Microsoft.Extensions.Logging;
using Quartz;

namespace BackgroundServices.Jobs.Refreshers;

public class RefresherJob : IJob
{
    private readonly IMediator _mediator;
    private readonly ILogger<RefresherJob> _logger;

    public RefresherJob(ILogger<RefresherJob> logger, IMediator mediator) => (_logger, _mediator) = (logger, mediator);

    public async Task Execute(IJobExecutionContext context) =>
        LogResult(
            await _mediator.Send(
                    new RefreshRatesCommand())
                .ConfigureAwait(false));

    private void LogResult(int result)
    {
        if (result >= 0)
        {
            _logger.LogInformation("Refreshing rates completed");
            return;
        }
        _logger.LogError("Refreshing rates failed");
    }
}
