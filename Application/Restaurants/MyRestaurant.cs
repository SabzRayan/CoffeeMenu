using Application.Core;
using Application.Interfaces;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Restaurants
{
    public class MyRestaurant
    {
        public class Query : IRequest<Result<RestaurantDto>>
        {
        }

        public class Handler : IRequestHandler<Query, Result<RestaurantDto>>
        {
            private readonly DataContext context;
            private readonly IMapper mapper;
            private readonly IUserAccessor userAccessor;

            public Handler(DataContext context, IMapper mapper, IUserAccessor userAccessor)
            {
                this.context = context;
                this.mapper = mapper;
                this.userAccessor = userAccessor;
            }

            public async Task<Result<RestaurantDto>> Handle(Query request, CancellationToken cancellationToken)
            {
                var restaurant = await context.Restaurants.Where(a => !a.IsDeleted && a.Users.Any(b => b.Id == userAccessor.GetUserId()))
                    .ProjectTo<RestaurantDto>(mapper.ConfigurationProvider)
                    .FirstOrDefaultAsync(cancellationToken: cancellationToken);

                return Result<RestaurantDto>.Success(restaurant);
            }
        }
    }
}
