using Application.Core;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Categories
{
    public class FullListCategory
    {
        public class Query : IRequest<Result<List<CategoryDto>>> 
        {
            public CategoryParams CategoryParams { get; set; }
        }

        public class Handler : IRequestHandler<Query, Result<List<CategoryDto>>>
        {
            private readonly DataContext context;
            private readonly IMapper mapper;

            public Handler(DataContext context, IMapper mapper)
            {
                this.context = context;
                this.mapper = mapper;
            }

            public async Task<Result<List<CategoryDto>>> Handle(Query request, CancellationToken cancellationToken)
            {
                var categoryList = await context.Categories
                    .Where(a => !a.IsDeleted &&
                                (request.CategoryParams.ParentId == null || a.ParentId == request.CategoryParams.ParentId) &&
                                (request.CategoryParams.BranchId == null || a.Branches.Any(b => b.BranchId == request.CategoryParams.BranchId)) &&
                                (request.CategoryParams.RestaurantId == null || a.RestaurantId == request.CategoryParams.RestaurantId))
                    .ProjectTo<CategoryDto>(mapper.ConfigurationProvider)
                    .ToListAsync(cancellationToken);

                return Result<List<CategoryDto>>.Success(categoryList);
            }
        }
    }
}
