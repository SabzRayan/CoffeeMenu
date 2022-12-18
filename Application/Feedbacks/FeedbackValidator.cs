using Domain;
using FluentValidation;

namespace Application.Feedbacks
{
    public class FeedbackValidator : AbstractValidator<Feedback>
    {
        public FeedbackValidator()
        {
            RuleFor(a => a.Happiness).IsInEnum();
            RuleFor(a => a.OrderDetailId).NotEmpty();
        }
    }
}
