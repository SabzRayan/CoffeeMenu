using Domain;
using FluentValidation;

namespace Application.Branches
{
    public class BranchCategoryValidator : AbstractValidator<BranchCategory>
    {
        public BranchCategoryValidator()
        {
            RuleFor(a => a.BranchId).NotEmpty();
            RuleFor(b => b.CategoryId).NotEmpty();
        }
    }
}
