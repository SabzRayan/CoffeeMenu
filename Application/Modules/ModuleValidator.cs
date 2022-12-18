using Domain;
using FluentValidation;

namespace Application.Modules
{
    public class ModuleValidator : AbstractValidator<Module>
    {
        public ModuleValidator()
        {
            RuleFor(a => a.Description).NotEmpty();
            RuleFor(a => a.Name).NotEmpty();
            RuleFor(a => a.Price).GreaterThan(0);
        }
    }
}
