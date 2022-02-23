using System.Threading.Tasks;

namespace RateGetters.Converters;

public interface IConverter
{
    public Task<decimal> Convert(ConvertCurrencySpecification specification);
}
