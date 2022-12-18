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

namespace Application.Branches
{
    public class BranchDetails
    {
        public class Query : IRequest<Result<BranchDto>>
        {
            public Guid Id { get; set; }
        }

        public class Handler : IRequestHandler<Query, Result<BranchDto>>
        {
            private readonly DataContext context;
            private readonly IMapper mapper;

            public Handler(DataContext context, IMapper mapper)
            {
                this.context = context;
                this.mapper = mapper;
            }

            public async Task<Result<BranchDto>> Handle(Query request, CancellationToken cancellationToken)
            {
                var branch = await context.Branches.Where(a => !a.IsDeleted)
                    .ProjectTo<BranchDto>(mapper.ConfigurationProvider)
                    .FirstOrDefaultAsync(a => a.Id == request.Id, cancellationToken: cancellationToken);

                return Result<BranchDto>.Success(branch);
            }
        }
    }
}
