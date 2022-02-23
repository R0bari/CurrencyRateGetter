using System.Threading;
using System.Threading.Tasks;
using Mapster;
using MapsterMapper;
using MediatR;
using Mongo.Contexts;
using RateGetters.Contexts;
using RateGetters.Rates.Models;
using RateGetters.Rates.Services.Interfaces;

namespace DomainServices.Queries.Currencies.Rates.GetForDate;

public class GetForDateHandler : IRequestHandler<GetForDateQuery, RateForDate>
{
    private readonly IRateService _rateService;
    private readonly IContext _context;
    private readonly IMapper _mapper;
    public GetForDateHandler(IRateService rateService, IContext context, IMapper mapper)
    {
        _rateService = rateService;
        _context = context;
        _mapper = mapper;
    }

    public async Task<RateForDate> Handle(GetForDateQuery request, CancellationToken cancellationToken) =>
        (await _context.GetRateForDate(
                request.Specification.Code,
                request.Specification.Date)
            .ConfigureAwait(false))
        .Adapt<RateForDate>();
}
