using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;
using RateGetters.Rates.Interfaces;
using RateGetters.Rates.Services;
using WebAPI.Infrastructure;

namespace WebAPI
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
        }

        public static void ConfigureServices(IServiceCollection services)
        {
            services
                .AddControllers()
                .AddNewtonsoftJson(o =>
                {
                    o.SerializerSettings.Converters.Add(
                        new StringEnumConverter{NamingStrategy = new CamelCaseNamingStrategy()});
                });
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo {Title = "WebAPI", Version = "v1"});
                c.EnableAnnotations();
                c.SchemaFilter<EnumSchemaFilter>();
            });
            services.AddTransient<IRateService, CbrRateService>();
        }

        public static void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "WebAPI v1"));
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
        }
    }
}