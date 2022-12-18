using Domain;
using FluentValidation;

namespace Application.Cities
{
    public class CityValidator : AbstractValidator<City>
    {
        public CityValidator()
        {
            RuleFor(a => a.Name).NotEmpty();
            RuleFor(a => a.ProvinceId).NotEmpty();
        }
    }
}
