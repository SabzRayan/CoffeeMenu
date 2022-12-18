using Domain;
using FluentValidation;
using System.Linq;

namespace Application.Categories
{
    public class CategoryValidator : AbstractValidator<Category>
    {
        public CategoryValidator()
        {
            RuleFor(c => c.Name).NotEmpty();
            RuleFor(c => c.RestaurantId).NotEmpty();
            RuleFor(c => c.Attachments.Count).GreaterThan(0);
            RuleFor(c => c.Attachments.Count(a => a.IsMain)).Equal(1).OverridePropertyName("Main Attachment Count");
        }
    }
}
