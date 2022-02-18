using Microsoft.Extensions.Caching.Memory;
using Mongo.Contexts;
using RateGetters.Rates.Services;
using RateGetters.Rates.Services.Interfaces;

namespace BackgroundServices;

public class Worker : BackgroundService
{
    private readonly ILogger<Worker> _logger;
    private readonly IRateService _rateService;
    private readonly IContext _context;

    public Worker(ILogger<Worker> logger, IContext context)
    {
        _logger = logger;
        _context = context;
        _rateService =
            new CachedCbrRateService(
                new MemoryCache(
                    new MemoryCacheOptions()));
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        //  Call service here
    }
}
