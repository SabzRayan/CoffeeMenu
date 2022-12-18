using Application.Core;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Persistence;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Modules
{
    public class ListModules
    {
        public class Query : IRequest<Result<PagedList<ModuleDto>>>
        {
            public ModuleParams Params { get; set; }
        }

        public class Handler : IRequestHandler<Query, Result<PagedList<ModuleDto>>>
        {
            private readonly DataContext context;
            private readonly IMapper mapper;

            public Handler(DataContext context, IMapper mapper)
            {
                this.context = context;
                this.mapper = mapper;
            }

            public async Task<Result<PagedList<ModuleDto>>> Handle(Query request, CancellationToken cancellationToken)
            {
                var query = context.Modules
                    .Where(a => (request.Params.ShowDeleted || !a.IsDeleted) &&
                                (request.Params.RestaurantId == null || a.Restaurants.Any(b => b.Id == request.Params.RestaurantId)))
                    .ProjectTo<ModuleDto>(mapper.ConfigurationProvider)
                    .AsQueryable();

                return Result<PagedList<ModuleDto>>.Success(
                    await PagedList<ModuleDto>.CreateAsync(query, request.Params.PageNumber, request.Params.PageSize)
                );
            }
        }
    }
}
