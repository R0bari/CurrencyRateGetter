using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
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

            var jobKey = new JobKey("Refresher Rates Job", "mainGroup");
            q.AddJob<RefresherJob>(jobKey, j => j.WithDescription("Refreshing rates"));

            q.AddTrigger(t => t
                .WithIdentity("Start now trigger")
                .ForJob(jobKey)
                .StartNow()
                .WithDescription("Triggers when the application starts up"));

            q.AddTrigger(t => t
                .WithIdentity("Start every day trigger")
                .ForJob(jobKey)
                .StartAt(DateTime.Today.AddDays(1))
                .WithSimpleSchedule(x => x
                    .WithIntervalInHours(24)
                    .RepeatForever())
                .WithDescription("Triggers every day at 0:00 (UTC+3)"));
        });
                
        services.AddQuartzHostedService(options =>
        {
            options.WaitForJobsToComplete = true;
        });

        services.AddTransient<RefresherJob>();
        services.AddTransient<ILogger, Logger<RefresherJob>>();
    }
}
