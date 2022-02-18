using Mapster;
using Mongo.Contexts;
using Mongo.Models;
using RateGetters.Rates.Models.Enums;
using RateGetters.Rates.Services.Interfaces;

namespace BackgroundServices;

public class RefreshRates
{
    private readonly ILogger<Worker> _logger;
    private readonly IRateService _rateService;
    private readonly IContext _context;

    private readonly CurrencyCodesEnum[] _currencies =
    {
        CurrencyCodesEnum.Eur,
        CurrencyCodesEnum.Usd,
        CurrencyCodesEnum.Rub
    };

    public RefreshRates(IRateService rateService, IContext context, ILogger<Worker> logger)
    {
        _rateService = rateService;
        _context = context;
        _logger = logger;
    }

    public async Task RefreshAll()
    {
        await _context.RemoveAllRates(true);
        var startDate = new DateTime(2000, 01, 01).Date;
        var totalDays = (int) (DateTime.Today - startDate).TotalDays;

        var rates = new List<RateForDate>();

        foreach (var currency in _currencies)
        {
            rates.AddRange(
                (await _rateService
                    .GetRatesForPeriodAsync(startDate, DateTime.Today, currency)
                    .ConfigureAwait(false)).Select(el => el.Adapt<RateForDate>()));
        }

        rates = (await FillGaps(rates, startDate, totalDays)
            .ConfigureAwait(false))
            .ToList();
                        
        await _context
            .InsertRatesForDate(rates)
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
            for (var j = 0; j <= totalDays; ++j)
            {
                var date = startDate.AddDays(j);

                if (rates[i * totalDays + j].DateTime == date)
                {
                    continue;
                }

                var rate = await _rateService
                    .GetRateAsync(date, _currencies[i])
                    .ConfigureAwait(false);
                rates.Insert(i * totalDays + j, rate.Adapt<RateForDate>());
            }
        }

        return rates;
    }
}
