using Application.Core;
using Application.Interfaces;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Domain.Enum;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Users
{
    public class MyUsers
    {
        public class Query : IRequest<Result<PagedList<UserDto>>>
        {
            public UserParams Params { get; set; }
        }

        public class Handler : IRequestHandler<Query, Result<PagedList<UserDto>>>
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

            public async Task<Result<PagedList<UserDto>>> Handle(Query request, CancellationToken cancellationToken)
            {
                var myRestaurant = await context.Restaurants.FirstOrDefaultAsync(a => a.Users.Any(b => b.Role == RoleEnum.Manager && b.Id == userAccessor.GetUserId()), cancellationToken: cancellationToken);
                var query = context.Users
                    .Where(a => !a.IsDeleted && a.RestaurantId == myRestaurant.Id &&
                                (request.Params.BranchId == null || a.BranchId == request.Params.BranchId) &&
                                (string.IsNullOrEmpty(request.Params.Name) || a.Name.Contains(request.Params.Name)) &&
                                (request.Params.RestaurantId == null || a.RestaurantId == request.Params.RestaurantId) &&
                                (request.Params.Role == null || a.Role == request.Params.Role))
                    .ProjectTo<UserDto>(mapper.ConfigurationProvider)
                    .AsQueryable();

                return Result<PagedList<UserDto>>.Success(
                    await PagedList<UserDto>.CreateAsync(
                        query,
                        request.Params.PageNumber,
                        request.Params.PageSize)
                );
            }
        }
    }
}
