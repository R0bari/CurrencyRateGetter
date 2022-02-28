using System.Threading.Tasks;

namespace DomainServices.Services.Converters;

public interface IConverter
{
    public Task<decimal> Convert(ConvertCurrencySpecification specification);
}
