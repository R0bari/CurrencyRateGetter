using System.Reflection;
using DomainServices.Services.Converters;
using DomainServices.Services.Rates;
using DomainServices.Services.Rates.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using MediatR;

namespace DomainServices.DependencyInjection;

public static class ServiceCollectionExtensions
{
    public static void AddDomainServices(this IServiceCollection services)
    {
        services.AddMediatR(Assembly.GetExecutingAssembly());
        services.AddTransient<IRateService, CachedCbrRateService>();
        services.AddTransient<IConverter, BaseConverter>();
    }
}
