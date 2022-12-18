using Application.Core;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Persistence;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Categories
{
    public class ListCategory
    {
        public class Query : IRequest<Result<PagedList<CategoryDto>>>
        {
            public CategoryParams CategoryParams { get; set; }
        }

        public class Handler : IRequestHandler<Query, Result<PagedList<CategoryDto>>>
        {
            private readonly DataContext context;
            private readonly IMapper mapper;

            public Handler(DataContext context, IMapper mapper)
            {
                this.context = context;
                this.mapper = mapper;
            }

            public async Task<Result<PagedList<CategoryDto>>> Handle(Query request, CancellationToken cancellationToken)
            {
                var query = context.Categories
                    .Where(a => !a.IsDeleted &&
                                (request.CategoryParams.ParentId == null || a.ParentId == request.CategoryParams.ParentId) &&
                                (request.CategoryParams.BranchId == null || a.Branches.Any(b => b.Id == request.CategoryParams.BranchId)) &&
                                (request.CategoryParams.RestaurantId == null || a.RestaurantId == request.CategoryParams.RestaurantId))
                    .ProjectTo<CategoryDto>(mapper.ConfigurationProvider)
                    .AsQueryable();

                return Result<PagedList<CategoryDto>>.Success(
                    await PagedList<CategoryDto>.CreateAsync(
                        query,
                        request.CategoryParams.PageNumber,
                        request.CategoryParams.PageSize)
                );
            }
        }
    }
}
