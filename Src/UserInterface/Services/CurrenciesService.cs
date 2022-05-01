using Domain.Models.Rates;
using Domain.Models.Rates.Enums;
using DomainServices.Queries.Currencies.Rates.GetAllRatesForDate;
using DomainServices.Queries.Currencies.Rates.GetForPeriod;
using MediatR;

namespace UserInterface.Services
{
    public class CurrenciesService
    {
        private readonly IMediator _mediator;

        public CurrenciesService(IMediator mediator) => _mediator = mediator;
        public List<CurrencyCodesEnum> GetCurrencyCodes() =>
            Enum
                .GetValues(typeof(CurrencyCodesEnum))
                .Cast<CurrencyCodesEnum>()
                .Where(c => c != CurrencyCodesEnum.None)
                .ToList();

        public async Task<List<RateForDate>> GetAllRatesForDate(DateTime date) =>
            await _mediator
                .Send(new GetAllRatesForDateQuery(
                    new GetAllRatesForDateSpecification(date)))
                .ConfigureAwait(false);

        public async Task<List<RateForDate>> GetRatesForLastQuarter(CurrencyCodesEnum code) =>
                await GetRatesForPeriod(code, DateTime.Now.Subtract(new TimeSpan(3 * 30, 0, 0, 0)), DateTime.Now);
        public async Task<List<RateForDate>> GetRatesForLastYear(CurrencyCodesEnum code) =>
                await GetRatesForPeriod(code, DateTime.Now.Subtract(new TimeSpan(365, 0, 0, 0)), DateTime.Now);

        private async Task<List<RateForDate>> GetRatesForPeriod(CurrencyCodesEnum code, DateTime firstDate, DateTime secondDate) =>
            (await _mediator
                .Send(new GetForPeriodQuery(
                    new GetForPeriodSpecification(code, firstDate, secondDate)),
                CancellationToken.None)
                .ConfigureAwait(false)
            )
                .ToList();
    }
}
