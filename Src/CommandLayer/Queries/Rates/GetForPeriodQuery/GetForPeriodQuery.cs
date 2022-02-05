using MediatR;
using RateGetters.Rates.Models;

namespace CommandLayer.Queries.Rates.GetForPeriodQuery
{
    public class GetForPeriodQuery : IRequest<PeriodRateList>
    {
        public GetForPeriodSpecification Specification { get; }

        public GetForPeriodQuery(GetForPeriodSpecification specification) => Specification = specification;
    }
}