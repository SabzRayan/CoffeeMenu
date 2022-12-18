using Domain;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Branches
{
    public class BranchValidator : AbstractValidator<Branch>
    {
        public BranchValidator()
        {
            RuleFor(c => c.Name).NotEmpty();
            RuleFor(c => c.Phone).Length(11);
            RuleFor(c => c.TableCount).GreaterThanOrEqualTo(1);
            RuleFor(c => c.Lat).NotNull();
            RuleFor(c => c.Lng).NotNull();
            RuleFor(c => c.CityId).NotEmpty();
            RuleFor(c => c.ProvinceId).NotEmpty();
        }
    }
}
