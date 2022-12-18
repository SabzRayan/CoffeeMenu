using Domain;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Restaurants
{
    public class RestaurantValidator : AbstractValidator<Restaurant>
    {
        public RestaurantValidator()
        {
            RuleFor(c => c.Name).NotEmpty();
            RuleFor(c => c.Logo).NotEmpty();
            RuleFor(c => c.Users.Count).Equal(0);
        }
    }
}
