using MediatR;
using RateGetters.Rates.Models.Enums;

namespace CommandLayer.Queries.Currencies.Convert.ConvertCurrencyQuery
{
    public record ConvertCurrencySpecification(CurrencyCodesEnum From, CurrencyCodesEnum To, decimal BaseValue);

    public record ConvertCurrencyQuery(ConvertCurrencySpecification Specification) : IRequest<decimal>;
}