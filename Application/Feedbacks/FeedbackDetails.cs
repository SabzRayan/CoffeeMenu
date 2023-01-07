using Application.Core;
using Application.Interfaces;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Feedbacks
{
    public class FeedbackDetails
    {
        public class Query : IRequest<Result<FeedbackDto>>
        {
            public Guid Id { get; set; }
        }

        public class Handler : IRequestHandler<Query, Result<FeedbackDto>>
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

            public async Task<Result<FeedbackDto>> Handle(Query request, CancellationToken cancellationToken)
            {
                var me = await context.Users.FindAsync(new object[] { userAccessor.GetUserId() }, cancellationToken);
                var isItMine = await context.Feedbacks.FirstOrDefaultAsync(a => a.Id == request.Id && a.OrderDetail.Order.Branch.RestaurantId == me.RestaurantId, cancellationToken: cancellationToken);
                if (isItMine == null) return Result<FeedbackDto>.Failure("You can't see feedbacks of a restaurant made by someone else");
                var feedback = await context.Feedbacks
                    .ProjectTo<FeedbackDto>(mapper.ConfigurationProvider)
                    .FirstOrDefaultAsync(a => a.Id == request.Id, cancellationToken: cancellationToken);

                return Result<FeedbackDto>.Success(feedback);
            }
        }
    }
}
