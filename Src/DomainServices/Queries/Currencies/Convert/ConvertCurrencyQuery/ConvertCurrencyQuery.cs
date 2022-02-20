using MediatR;
using RateGetters.Rates.Models.Enums;

namespace DomainServices.Queries.Currencies.Convert.ConvertCurrencyQuery;

public record ConvertCurrencySpecification(CurrencyCodesEnum From, CurrencyCodesEnum To, decimal BaseValue);

public record ConvertCurrencyQuery(ConvertCurrencySpecification Specification) : IRequest<decimal>;
