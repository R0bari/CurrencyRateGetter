using Mapster;
using Microsoft.Extensions.Caching.Memory;
using Mongo.Contexts;
using Mongo.Models;
using Quartz;
using RateGetters.Rates.Models.Enums;
using RateGetters.Rates.Services;
using RateGetters.Rates.Services.Interfaces;

namespace BackgroundServices.Jobs.Refreshers;

public class RefresherJob : IJob
{
    private readonly IContext _context;
    private readonly IRateService _rateService =
        new CachedCbrRateService(
            new MemoryCache(
                new MemoryCacheOptions()));
    private readonly CurrencyCodesEnum[] _currencies =
    {
        CurrencyCodesEnum.Eur,
        CurrencyCodesEnum.Usd
    };

    public RefresherJob()
    {
        _context = new MongoContext();
    }

    public RefresherJob(IContext context)
    {
        _context = context;
    }

    public async Task Execute(IJobExecutionContext context)
    {
        var recentDate = await _context
            .GetMostRecentDate()
            .ConfigureAwait(false);
        if (recentDate == DateTime.Today)
        {
            return;
        }
        await RefreshFromDate(recentDate.AddDays(1))
            .ConfigureAwait(false);
    }

    private async Task RefreshFromDate(DateTime startDate)
    {
        var totalDays = (int) (DateTime.Today - startDate).TotalDays;

        var ratesToInsert = new List<RateForDate>();

        foreach (var currency in _currencies)
        {
            ratesToInsert.AddRange(
                (await _rateService
                    .GetRatesForPeriodAsync(startDate,  DateTime.Today, currency)
                    .ConfigureAwait(false)).Select(el => el.Adapt<RateForDate>()));
        }

        ratesToInsert = (await FillGaps(ratesToInsert, startDate, totalDays)
                .ConfigureAwait(false))
            .ToList();

        await _context
            .InsertRatesForDate(ratesToInsert)
            .ConfigureAwait(false);
    }
    
    private async Task<IEnumerable<RateForDate>> FillGaps(
        IEnumerable<RateForDate> ratesForDate,
        DateTime startDate,
        int totalDays)
    {
        var rates = ratesForDate.ToList();
        for (var i = 0; i < _currencies.Length; i++)
        {
            for (var j = 0; j < totalDays; ++j)
            {
                var date = startDate.AddDays(j);

                if (rates[i * (totalDays + 1) + j].Date == date)
                {
                    continue;
                }

                if (date != startDate)
                {
                    var previousRate = rates[i * (totalDays + 1) + j - 1];
                    rates.Insert(
                        i * (totalDays + 1) + j,
                        new RateForDate(previousRate.Code, previousRate.Value, date));
                    continue;
                }

                rates.Insert(
                    i * (totalDays + 1) + j,
                    (await _rateService
                        .GetRateAsync(date, _currencies[i])
                        .ConfigureAwait(false))
                    .Adapt<RateForDate>());
            }
        }

        return rates;
    }
}
