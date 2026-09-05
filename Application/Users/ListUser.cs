using Application.Core;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Persistence;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Users
{
    public class ListUser
    {
        public class Query : IRequest<Result<PagedList<UserDto>>>
        {
            public UserParams Params { get; set; }
        }

        public class Handler : IRequestHandler<Query, Result<PagedList<UserDto>>>
        {
            private readonly DataContext context;
            private readonly IMapper mapper;

            public Handler(DataContext context, IMapper mapper)
            {
                this.context = context;
                this.mapper = mapper;
            }

            public async Task<Result<PagedList<UserDto>>> Handle(Query request, CancellationToken cancellationToken)
            {
                var query = context.Users
                    .Where(a => !a.IsDeleted &&
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
