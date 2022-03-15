using BackgroundServices.Jobs.Refreshers;
using DataBase.Contexts;
using Domain.Contexts;
using DomainServices.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

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
                services.AddSingleton<IContext, MongoContext>();
            });
}
