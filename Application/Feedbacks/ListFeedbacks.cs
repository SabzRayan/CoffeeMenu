using Application.Core;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Persistence;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Feedbacks
{
    public class ListFeedbacks
    {
        public class Query : IRequest<Result<PagedList<FeedbackDto>>>
        {
            public FeedbackParams Params { get; set; }
        }

        public class Handler : IRequestHandler<Query, Result<PagedList<FeedbackDto>>>
        {
            private readonly DataContext context;
            private readonly IMapper mapper;

            public Handler(DataContext context, IMapper mapper)
            {
                this.context = context;
                this.mapper = mapper;
            }

            public async Task<Result<PagedList<FeedbackDto>>> Handle(Query request, CancellationToken cancellationToken)
            {
                var query = context.Feedbacks
                    .Where(a => (request.Params.ShowDeleted || !a.IsDeleted) &&
                                (request.Params.ProductId == null || a.OrderDetail.ProductId == request.Params.ProductId) &&
                                (request.Params.OrderId == null || a.OrderDetail.OrderId == request.Params.OrderId) &&
                                (request.Params.BranchId == null || a.OrderDetail.Order.BranchId == request.Params.BranchId))
                    .ProjectTo<FeedbackDto>(mapper.ConfigurationProvider)
                    .AsQueryable();

                return Result<PagedList<FeedbackDto>>.Success(
                    await PagedList<FeedbackDto>.CreateAsync(query, request.Params.PageNumber, request.Params.PageSize)
                );
            }
        }
    }
}
