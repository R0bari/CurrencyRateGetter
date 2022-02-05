using MediatR;
using RateGetters.Rates.Models;

namespace CommandLayer.Queries.Rates.GetForDateQuery
{
    public class GetForDateQuery : IRequest<RateForDate>
    {
        public GetForDateSpecification Specification { get; set; }

        public GetForDateQuery(GetForDateSpecification specification) =>
            Specification = specification;
    }
}