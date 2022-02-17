using System.Threading.Tasks;
using RateGetters.Rates.Models.Enums;

namespace RateGetters.Converters;

public interface IConverter
{
    public Task<decimal> Convert(CurrencyCodesEnum from, CurrencyCodesEnum to, decimal baseValue);
}