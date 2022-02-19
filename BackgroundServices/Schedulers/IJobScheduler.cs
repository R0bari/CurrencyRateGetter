using Quartz;

namespace BackgroundServices.Schedulers;

public interface IJobScheduler
{
    public Task Start();
    public Task Schedule(
        IJobDetail jobDetail,
        ITrigger trigger,
        CancellationToken token);

    public Task Shutdown(CancellationToken token);
}
