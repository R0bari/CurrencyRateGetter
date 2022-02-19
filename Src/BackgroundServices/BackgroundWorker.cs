using BackgroundServices.Jobs.Refreshers;
using BackgroundServices.Schedulers;
using Quartz;

namespace BackgroundServices;

public class BackgroundWorker : BackgroundService
{
    private readonly ILogger<BackgroundWorker> _logger;
    private readonly IJobScheduler _scheduler;

    public BackgroundWorker(ILogger<BackgroundWorker> logger, IJobScheduler scheduler)
    {
        _logger = logger;
        _scheduler = scheduler;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        await _scheduler.Start();
        var refresherJob = JobBuilder.Create<RefresherJob>()
            .Build();
        
        var trigger = TriggerBuilder.Create()
            .WithIdentity("refresherTrigger", "mainGroup")
            .StartAt(new DateTimeOffset(DateTime.Today.AddDays(1).AddHours(2)))
            .WithSimpleSchedule(x => x
                .WithIntervalInHours(24)
                .RepeatForever())
            .Build();

        await _scheduler.Schedule(refresherJob, trigger, stoppingToken);
    }
}
