using Application.Core;
using MediatR;
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

            public Handler(DataContext context)
            {
                this.context = context;
            }

            public async Task<Result<Unit>> Handle(Command request, CancellationToken cancellationToken)
            {
                var feedback = await context.Feedbacks.FindAsync(new object[] { request.Id }, cancellationToken: cancellationToken);
                if (feedback == null) return null;
                feedback.IsDeleted = true;
                var result = await context.SaveChangesAsync(cancellationToken) > 0;
                if (!result) return Result<Unit>.Failure("Failed to delete the feedback");
                return Result<Unit>.Success(Unit.Value);
            }
        }
    }
}
