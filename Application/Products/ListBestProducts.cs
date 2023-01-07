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

namespace Application.Products
{
    public class ListBestProduct
    {
        public class Query : IRequest<Result<List<ProductDto>>>
        {
            public ProductParams Params { get; set; }
        }

        public class Handler : IRequestHandler<Query, Result<List<ProductDto>>>
        {
            private readonly DataContext context;
            private readonly IMapper mapper;

            public Handler(DataContext context, IMapper mapper)
            {
                this.context = context;
                this.mapper = mapper;
            }

            public async Task<Result<List<ProductDto>>> Handle(Query request, CancellationToken cancellationToken)
            {
                var query = await context.Products
                    .Where(a => !a.IsDeleted && a.ChefSuggestion &&
                                (request.Params.BranchId == null || a.Category.Branches.Any(b => b.BranchId == request.Params.BranchId)) &&
                                (request.Params.RestaurantId == null || a.Category.RestaurantId == request.Params.RestaurantId))
                    .ProjectTo<ProductDto>(mapper.ConfigurationProvider)
                    .ToListAsync(cancellationToken);

                return Result<List<ProductDto>>.Success(query);
            }
        }
    }
}
