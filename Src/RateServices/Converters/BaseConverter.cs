using System;
using System.Threading.Tasks;
using RateGetters.Rates.Models.Enums;
using RateGetters.Rates.Services.Interfaces;

namespace RateGetters.Converters;

public class BaseConverter : IConverter
{
    private readonly IRateService _rateService;

    public BaseConverter(IRateService rateService) => _rateService = rateService;
    
    public Task<decimal> Convert(CurrencyCodesEnum from, CurrencyCodesEnum to, decimal baseValue)
    {
        var now = DateTime.Now;
        var fromRateTask = _rateService
            .GetRateAsync(now, from);
        var toRateTask = _rateService
            .GetRateAsync(now, to);

        Task.WaitAll(fromRateTask, toRateTask);
        return Task.FromResult(baseValue * fromRateTask.Result.Rate.Value / toRateTask.Result.Rate.Value);
    }
}