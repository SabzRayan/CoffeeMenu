using Application.Core;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Cities
{
    public class FullListCities
    {
        public class Query : IRequest<Result<List<CityDto>>>
        {
            public CityParams Params { get; set; }
        }

        public class Handler : IRequestHandler<Query, Result<List<CityDto>>>
        {
            private readonly DataContext context;
            private readonly IMapper mapper;

            public Handler(DataContext context, IMapper mapper)
            {
                this.context = context;
                this.mapper = mapper;
            }

            public async Task<Result<List<CityDto>>> Handle(Query request, CancellationToken cancellationToken)
            {
                var cities = await context.Cities
                    .Where(a => a.ProvinceId == request.Params.ProvinceId)
                    .OrderBy(a => a.Name)
                    .ProjectTo<CityDto>(mapper.ConfigurationProvider)
                    .ToListAsync(cancellationToken);

                return Result<List<CityDto>>.Success(cities);
            }
        }
    }
}
