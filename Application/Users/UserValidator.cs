using Domain;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Users
{
    public class UserValidator : AbstractValidator<User>
    {
        public UserValidator()
        {
            RuleFor(c => c.Name).NotEmpty();
            RuleFor(c => c.BranchId).NotEmpty();
            RuleFor(c => c.Email).EmailAddress().NotEmpty();
        }
    }
}
