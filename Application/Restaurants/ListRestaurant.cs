using Application.Core;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Persistence;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Restaurants
{
    public class ListRestaurant
    {
        public class Query : IRequest<Result<PagedList<RestaurantDto>>>
        {
            public RestaurantParams Params { get; set; }
        }

        public class Handler : IRequestHandler<Query, Result<PagedList<RestaurantDto>>>
        {
            private readonly DataContext context;
            private readonly IMapper mapper;

            public Handler(DataContext context, IMapper mapper)
            {
                this.context = context;
                this.mapper = mapper;
            }

            public async Task<Result<PagedList<RestaurantDto>>> Handle(Query request, CancellationToken cancellationToken)
            {
                var query = context.Restaurants
                    .Where(a => !a.IsDeleted &&
                                (string.IsNullOrEmpty(request.Params.Domain) || a.Domain == request.Params.Domain) &&
                                (request.Params.UserId == null || a.Users.Any(b => b.Id == request.Params.UserId)) &&
                                (string.IsNullOrEmpty(request.Params.Name) || a.Name.Contains(request.Params.Name)))
                    .ProjectTo<RestaurantDto>(mapper.ConfigurationProvider)
                    .AsQueryable();

                return Result<PagedList<RestaurantDto>>.Success(
                    await PagedList<RestaurantDto>.CreateAsync(
                        query,
                        request.Params.PageNumber,
                        request.Params.PageSize)
                );
            }
        }
    }
}
