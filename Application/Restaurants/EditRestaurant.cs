using Application.Core;
using Domain;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Restaurants
{
    public class EditRestaurant
    {
        public class Command : IRequest<Result<Unit>>
        {
            public Restaurant Restaurant { get; set; }
        }

        public class CommandValidator : AbstractValidator<Command>
        {
            public CommandValidator()
            {
                RuleFor(x => x.Restaurant).SetValidator(new RestaurantValidator());
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
                var restaurant = await context.Restaurants.FirstOrDefaultAsync(a => a.Id == request.Restaurant.Id, cancellationToken: cancellationToken);
                if (restaurant == null) return null;
                restaurant.Name = request.Restaurant.Name;
                restaurant.Logo= request.Restaurant.Logo;
                restaurant.HeaderPic = request.Restaurant.HeaderPic;
                restaurant.Description = request.Restaurant.Description;
                restaurant.About = request.Restaurant.About;
                restaurant.Address = request.Restaurant.Address;
                restaurant.Instagram = request.Restaurant.Instagram;
                restaurant.Facebook = request.Restaurant.Facebook;
                restaurant.Whatsapp = request.Restaurant.Whatsapp;
                restaurant.Telegram = request.Restaurant.Telegram;
                restaurant.Tweeter = request.Restaurant.Tweeter;

                var result = await context.SaveChangesAsync(cancellationToken) > 0;
                if (!result) return Result<Unit>.Failure("Failed to update restaurant");
                return Result<Unit>.Success(Unit.Value);
            }
        }
    }
}
