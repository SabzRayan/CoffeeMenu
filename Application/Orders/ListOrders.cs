using Application.Core;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Persistence;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Orders
{
    public class ListOrders
    {
        public class Query : IRequest<Result<PagedList<OrderDto>>>
        {
            public OrderParams Params { get; set; }
        }

        public class Handler : IRequestHandler<Query, Result<PagedList<OrderDto>>>
        {
            private readonly DataContext context;
            private readonly IMapper mapper;

            public Handler(DataContext context, IMapper mapper)
            {
                this.context = context;
                this.mapper = mapper;
            }

            public async Task<Result<PagedList<OrderDto>>> Handle(Query request, CancellationToken cancellationToken)
            {
                var query = context.Orders
                    .Where(a => !a.IsDeleted &&
                                (request.Params.BranchId == null || a.BranchId == request.Params.BranchId) &&
                                (request.Params.Status == null || a.Status == request.Params.Status) &&
                                (request.Params.FromDate == null || a.CreatedAt >= request.Params.FromDate) &&
                                (request.Params.ToDate == null || a.CreatedAt <= request.Params.ToDate))
                    .ProjectTo<OrderDto>(mapper.ConfigurationProvider)
                    .AsQueryable();

                return Result<PagedList<OrderDto>>.Success(
                    await PagedList<OrderDto>.CreateAsync(
                        query,
                        request.Params.PageNumber,
                        request.Params.PageSize)
                );
            }
        }
    }
}
