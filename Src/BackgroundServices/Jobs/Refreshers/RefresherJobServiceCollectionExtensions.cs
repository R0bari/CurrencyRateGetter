using Quartz;

namespace BackgroundServices.Jobs.Refreshers;

public static class RefresherJobServiceCollectionExtensions
{
    public static void AddRefresherJobScheduling(this IServiceCollection services)
    {
        services.AddQuartz(q =>
        {
            q.SchedulerId = "RefresherJob-Scheduler";
                    
            q.UseMicrosoftDependencyInjectionJobFactory();
                    
            q.UseSimpleTypeLoader();
            q.UseInMemoryStore();
            q.UseDefaultThreadPool(tp =>
            {
                tp.MaxConcurrency = 10;
            });

            q.ScheduleJob<RefresherJob>(trigger => trigger
                .WithIdentity("RefresherTrigger", "mainGroup")
                .StartAt(DateTime.Today.AddDays(1).AddHours(2))
                .WithSimpleSchedule(x => x
                    .WithIntervalInHours(24)
                    .RepeatForever())
                .WithDescription("Triggers every day at 2:00 (UTC+3)"));

            q.AddJob<RefresherJob>(j => j
                .StoreDurably()
                .WithDescription("Refreshing rates"));
        });
                
        services.AddQuartzHostedService(options =>
        {
            options.WaitForJobsToComplete = true;
        });

        services.AddTransient<RefresherJob>();
        services.AddTransient<ILogger, Logger<RefresherJob>>();
    }
}
