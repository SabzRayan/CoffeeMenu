using Application.Core;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Persistence;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Cities
{
    public class ListCities
    {
        public class Query : IRequest<Result<PagedList<CityDto>>>
        {
            public CityParams Params { get; set; }
        }

        public class Handler : IRequestHandler<Query, Result<PagedList<CityDto>>>
        {
            private readonly DataContext context;
            private readonly IMapper mapper;

            public Handler(DataContext context, IMapper mapper)
            {
                this.context = context;
                this.mapper = mapper;
            }

            public async Task<Result<PagedList<CityDto>>> Handle(Query request, CancellationToken cancellationToken)
            {
                var query = context.Cities
                    .Where(a => a.ProvinceId == request.Params.ProvinceId)
                    .OrderBy(a => a.Name)
                    .ProjectTo<CityDto>(mapper.ConfigurationProvider)
                    .AsQueryable();

                return Result<PagedList<CityDto>>.Success(
                    await PagedList<CityDto>.CreateAsync(query, request.Params.PageNumber, request.Params.PageSize)
                );
            }
        }
    }
}
