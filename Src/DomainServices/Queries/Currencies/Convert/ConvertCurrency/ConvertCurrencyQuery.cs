using System.Threading;
using System.Threading.Tasks;
using MediatR;
using RateGetters.Converters;

namespace DomainServices.Queries.Currencies.Convert.ConvertCurrency;

public record ConvertCurrencyQuery(ConvertCurrencySpecification Specification) : IRequest<decimal>;
public class ConvertCurrencyHandler : IRequestHandler<ConvertCurrencyQuery, decimal>
{
    private readonly IConverter _converter;

    public ConvertCurrencyHandler(IConverter converter) => _converter = converter;
        
    public async Task<decimal> Handle(ConvertCurrencyQuery request, CancellationToken cancellationToken) =>
        await _converter.Convert(request.Specification)
            .ConfigureAwait(false);
}
