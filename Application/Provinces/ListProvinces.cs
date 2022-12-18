using Application.Core;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Persistence;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Provinces
{
    public class ListProvinces
    {
        public class Query : IRequest<Result<PagedList<ProvinceDto>>> 
        {
            public PagingParams Params { get; set; }
        }

        public class Handler : IRequestHandler<Query, Result<PagedList<ProvinceDto>>>
        {
            private readonly DataContext context;
            private readonly IMapper mapper;

            public Handler(DataContext context, IMapper mapper)
            {
                this.context = context;
                this.mapper = mapper;
            }

            public async Task<Result<PagedList<ProvinceDto>>> Handle(Query request, CancellationToken cancellationToken)
            {
                var query = context.Provinces
                    .OrderBy(a => a.Name)
                    .ProjectTo<ProvinceDto>(mapper.ConfigurationProvider)
                    .AsQueryable();

                return Result<PagedList<ProvinceDto>>.Success(
                    await PagedList<ProvinceDto>.CreateAsync(query, request.Params.PageNumber, request.Params.PageSize)
                );
            }
        }
    }
}
