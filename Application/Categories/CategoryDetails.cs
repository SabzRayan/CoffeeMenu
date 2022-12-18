using Application.Core;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Categories
{
    public class CategoryDetails
    {
        public class Query : IRequest<Result<CategoryDto>>
        {
            public Guid Id { get; set; }
        }

        public class Handler : IRequestHandler<Query, Result<CategoryDto>>
        {
            private readonly DataContext context;
            private readonly IMapper mapper;

            public Handler(DataContext context, IMapper mapper)
            {
                this.context = context;
                this.mapper = mapper;
            }

            public async Task<Result<CategoryDto>> Handle(Query request, CancellationToken cancellationToken)
            {
                var category = await context.Categories.Where(a => !a.IsDeleted)
                    .ProjectTo<CategoryDto>(mapper.ConfigurationProvider)
                    .FirstOrDefaultAsync(a => a.Id == request.Id, cancellationToken: cancellationToken);

                return Result<CategoryDto>.Success(category);
            }
        }
    }
}
