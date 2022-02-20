using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Mapster;
using MediatR;
using Mongo.Contexts;
using Mongo.Models;
using RateGetters.Rates.Models.Enums;
using RateGetters.Rates.Services.Interfaces;

namespace DomainServices.Commands.Rates.Refresh.RefreshFromDate;

public class RefreshRatesFromDateHandler : IRequestHandler<RefreshRatesFromDateCommand, int>
{
    private readonly IRateService _rateService;
    private readonly IContext _context;
    private readonly CurrencyCodesEnum[] _currencies;


    public RefreshRatesFromDateHandler(IRateService rateService, IContext context)
    {
        _currencies = Enum.GetValues(typeof(CurrencyCodesEnum)).Cast<CurrencyCodesEnum>()
            .Where(currency => currency != CurrencyCodesEnum.None && currency != CurrencyCodesEnum.Rub)
            .ToArray();
        _rateService = rateService;
        _context = context;
    }

    public async Task<int> Handle(RefreshRatesFromDateCommand request, CancellationToken cancellationToken)
    {
        var result = await RefreshFromDate(request.Date)
            .ConfigureAwait(false);
        return result;
    }

    private async Task<int> RefreshFromDate(DateTime startDate)
    {
        var totalDays = (int) (DateTime.Today - startDate).TotalDays + 1;

        var ratesToInsert = new List<RateForDate>();

        foreach (var currency in _currencies)
        {
            var currencyRates = (await _rateService
                    .GetRatesForPeriodAsync(startDate, DateTime.Today, currency)
                    .ConfigureAwait(false))
                .Select(el => el.Adapt<RateForDate>());
            currencyRates = await FillGaps(currencyRates, currency, startDate, totalDays)
                .ConfigureAwait(false);
            ratesToInsert.AddRange(currencyRates);
        }

        return await _context
            .InsertRateForDateList(ratesToInsert)
            .ConfigureAwait(false);
    }

    private async Task<IEnumerable<RateForDate>> FillGaps(
        IEnumerable<RateForDate> ratesForDate,
        CurrencyCodesEnum code,
        DateTime startDate,
        int totalDays)
    {
        var rates = ratesForDate.ToList();

        for (var i = 0; i < totalDays; ++i)
        {
            var date = startDate.AddDays(i);

            if (i == rates.Count)
            {
                var prevRate = rates[i - 1];
                rates.Insert(
                    i,
                    new RateForDate(prevRate.Code, prevRate.Value, date));
                continue;
            }
            
            if (rates[i].Date == date)
            {
                continue;
            }
            
            if (date == startDate)
            {
                rates.Insert(
                    i,
                    (await _rateService
                        .GetRateAsync(date, code)
                        .ConfigureAwait(false))
                    .Adapt<RateForDate>());
                continue;
            }

            var previousRate = rates[i - 1];
            rates.Insert(
                i,
                new RateForDate(previousRate.Code, previousRate.Value, date));
        }


        return rates;
    }
}
