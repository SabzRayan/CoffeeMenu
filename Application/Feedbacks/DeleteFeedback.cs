using Application.Core;
using Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Feedbacks
{
    public class DeleteFeedback
    {
        public class Command : IRequest<Result<Unit>>
        {
            public Guid Id { get; set; }
        }

        public class Handler : IRequestHandler<Command, Result<Unit>>
        {
            private readonly DataContext context;
            private readonly IUserAccessor userAccessor;

            public Handler(DataContext context, IUserAccessor userAccessor)
            {
                this.context = context;
                this.userAccessor = userAccessor;
            }

            public async Task<Result<Unit>> Handle(Command request, CancellationToken cancellationToken)
            {
                var feedback = await context.Feedbacks.Include(a => a.OrderDetail.Order.Branch).FirstOrDefaultAsync(a => a.Id == request.Id, cancellationToken: cancellationToken);
                if (feedback == null) return null;
                var me = await context.Users.FindAsync(new object[] { userAccessor.GetUserId() }, cancellationToken);
                if (me.RestaurantId != feedback.OrderDetail.Order.Branch.RestaurantId) return Result<Unit>.Failure("You can't delete a feedback belongs to someone else restaurant");
                feedback.IsDeleted = true;
                var result = await context.SaveChangesAsync(cancellationToken) > 0;
                if (!result) return Result<Unit>.Failure("Failed to delete the feedback");
                return Result<Unit>.Success(Unit.Value);
            }
        }
    }
}
