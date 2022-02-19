using BackgroundServices.Schedulers;
using Mongo.Contexts;

namespace BackgroundServices;

public static class Program
{
    public static void Main(string[] args)
    {
        CreateHostBuilder(args).Build().Run();
    }

    private static IHostBuilder CreateHostBuilder(string[] args) =>
        Host.CreateDefaultBuilder(args)
            .ConfigureServices(services =>
            {
                services.AddHostedService<BackgroundWorker>();
                services.AddTransient<ILogger, Logger<BackgroundWorker>>();
                services.AddTransient<IContext, MongoContext>();
                services.AddScoped<IJobScheduler, JobScheduler>();
            });
}
