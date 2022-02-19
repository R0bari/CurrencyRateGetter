using Quartz;
using Quartz.Impl;

namespace BackgroundServices.Schedulers;

public class JobScheduler : IJobScheduler
{
    private readonly IScheduler _scheduler;
    public JobScheduler()
    {
        var factory = new StdSchedulerFactory();
        _scheduler = factory
            .GetScheduler(CancellationToken.None)
            .GetAwaiter()
            .GetResult();
    }

    public Task Start() => _scheduler.Start();

    public Task Shutdown(CancellationToken token) => _scheduler.Shutdown(token);
    public async Task Schedule(IJobDetail jobDetail, ITrigger trigger, CancellationToken token)
    {
        await _scheduler
            .ScheduleJob(jobDetail, trigger, token)
            .ConfigureAwait(false);
    }
}
