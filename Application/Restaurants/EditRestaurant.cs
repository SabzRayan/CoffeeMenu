using Application.Core;
using Application.Interfaces;
using Domain;
using Domain.Enum;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;
using System.Linq;
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
            private readonly IUserAccessor userAccessor;

            public Handler(DataContext context, IUserAccessor userAccessor)
            {
                this.context = context;
                this.userAccessor = userAccessor;
            }

            public async Task<Result<Unit>> Handle(Command request, CancellationToken cancellationToken)
            {
                var restaurant = await context.Restaurants.Include(a => a.Users).FirstOrDefaultAsync(a => a.Id == request.Restaurant.Id, cancellationToken: cancellationToken);
                if (restaurant == null) return null;
                if (restaurant.Users.FirstOrDefault(a => a.Role == RoleEnum.Manager).Id != userAccessor.GetUserId()) return Result<Unit>.Failure("You can't edit restaurants made by someone else");
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
