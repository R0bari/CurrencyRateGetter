using System.Reflection;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using RateGetters.Converters;
using RateGetters.Rates.Services;
using RateGetters.Rates.Services.Interfaces;

namespace CommandLayer.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {
        public static void AddCommandLayerServices(this IServiceCollection services)
        {
            services.AddMediatR(Assembly.GetExecutingAssembly());
            services.AddTransient<IRateService, CachedCbrRateService>();
            services.AddTransient<IConverter, BaseConverter>();
        }
    }
}