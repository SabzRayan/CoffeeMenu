using Application.Core;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Persistence;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Branches
{
    public class ListBranch
    {
        public class Query : IRequest<Result<PagedList<BranchDto>>>
        {
            public BranchParams Params { get; set; }
        }

        public class Handler : IRequestHandler<Query, Result<PagedList<BranchDto>>>
        {
            private readonly DataContext context;
            private readonly IMapper mapper;
            private readonly double maxDistance = 0.0005;

            public Handler(DataContext context, IMapper mapper)
            {
                this.context = context;
                this.mapper = mapper;
            }

            public async Task<Result<PagedList<BranchDto>>> Handle(Query request, CancellationToken cancellationToken)
            {
                var query = context.Branches
                    .Where(a => !a.IsDeleted &&
                                (request.Params.CityId == null || a.CityId == request.Params.CityId) &&
                                (request.Params.Lat == null || (((a.Lat.CompareTo(request.Params.Lat) - maxDistance) < maxDistance) && ((a.Lng.CompareTo(request.Params.Lng) - maxDistance) < maxDistance))) &&
                                (string.IsNullOrEmpty(request.Params.Name) || a.Name.Contains(request.Params.Name)) &&
                                (request.Params.ProvinceId == null || a.ProvinceId == request.Params.ProvinceId) &&
                                (request.Params.RestaurantId == null || a.RestaurantId == request.Params.RestaurantId))
                    .ProjectTo<BranchDto>(mapper.ConfigurationProvider)
                    .AsQueryable();

                return Result<PagedList<BranchDto>>.Success(
                    await PagedList<BranchDto>.CreateAsync(
                        query,
                        request.Params.PageNumber,
                        request.Params.PageSize)
                );
            }
        }
    }
}
