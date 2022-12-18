using Application.Core;
using Domain;
using FluentValidation;
using MediatR;
using Persistence;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Feedbacks
{
    public class EditFeedback
    {
        public class Command : IRequest<Result<Unit>>
        {
            public Feedback Feedback { get; set; }
        }

        public class CommandValidator : AbstractValidator<Command>
        {
            public CommandValidator()
            {
                RuleFor(a => a.Feedback).SetValidator(new FeedbackValidator());
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
                var feedback = await context.Feedbacks.FindAsync(new object[] { request.Feedback.Id }, cancellationToken);
                if (feedback == null) return null;
                feedback.ReportText = request.Feedback.ReportText;
                feedback.Happiness = request.Feedback.Happiness;
                var result = await context.SaveChangesAsync(cancellationToken) > 0;
                if (!result) return Result<Unit>.Failure("Failed to update feedback");
                return Result<Unit>.Success(Unit.Value);
            }
        }
    }
}
