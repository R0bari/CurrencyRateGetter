using BackgroundServices.Jobs.Refreshers;
using DomainServices.DependencyInjection;

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
                services.AddDomainServices();
                services.AddRefresherJobScheduling();
            });
}
