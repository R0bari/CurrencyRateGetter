using System;
using System.Threading.Tasks;
using RateGetters.Rates.Models.Enums;
using RateGetters.Rates.Services.Interfaces;

namespace RateGetters.Converters;


public record ConvertCurrencySpecification(CurrencyCodesEnum From, CurrencyCodesEnum To, decimal BaseValue);

public class BaseConverter : IConverter
{
    private readonly IRateService _rateService;

    public BaseConverter(IRateService rateService) => _rateService = rateService;
    
    public Task<decimal> Convert(ConvertCurrencySpecification specification)
    {
        var now = DateTime.Now;
        var fromRateTask = _rateService
            .GetRateAsync(now, specification.From);
        var toRateTask = _rateService
            .GetRateAsync(now, specification.To);

        Task.WaitAll(fromRateTask, toRateTask);

        var fromRateValue = fromRateTask.Result.Value;
        var toRateValue = toRateTask.Result.Value;
        return toRateValue != 0
            ? Task.FromResult(specification.BaseValue * fromRateValue / toRateValue)
            : Task.FromResult(0m);
    }
}
