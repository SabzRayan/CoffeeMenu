using Application.Core;
using Domain;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Feedbacks
{
    public class CreateFeedback
    {
        public class Command : IRequest<Result<Unit>>
        {
            public Feedback Feedback { get; set; }
        }

        public class CommandValidator : AbstractValidator<Command>
        {
            public CommandValidator() 
            {
                RuleFor(x => x.Feedback).SetValidator(new FeedbackValidator());
            }
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
                var orderDetail = await context.OrderDetails.Include(a => a.Feedbacks).Include(a => a.Order).FirstOrDefaultAsync(a => a.Id == request.Feedback.OrderDetailId, cancellationToken: cancellationToken);
                if (orderDetail == null) return null;
                if (orderDetail.Feedbacks.Any()) return Result<Unit>.Failure("Already registered");
                if (orderDetail.Order.CreatedAt < DateTime.UtcNow.AddDays(-1)) return Result<Unit>.Failure("Too late to send a feedback");
                await context.Feedbacks.AddAsync(request.Feedback, cancellationToken);
                var result = await context.SaveChangesAsync(cancellationToken) > 0;
                if (!result) return Result<Unit>.Failure("Failed to create feedback");
                return Result<Unit>.Success(Unit.Value);
            }
        }
    }
}
