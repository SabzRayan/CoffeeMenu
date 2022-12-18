using Application.Core;
using Application.Interfaces;
using Domain;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Restaurants
{
    public class CreateRestaurant
    {
        public class Command : IRequest<Result<Unit>>
        {
            public Restaurant Restaurant { get; set; }
        }

        public class CommandValidator : AbstractValidator<Command>
        {
            public CommandValidator()
            {
                RuleFor(a => a.Restaurant).SetValidator(new RestaurantValidator());
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
                var owner = await context.Users.FirstOrDefaultAsync(a => a.UserName == userAccessor.GetUsername(), cancellationToken: cancellationToken);
                request.Restaurant.Users.Add(owner);
                await context.Restaurants.AddAsync(request.Restaurant, cancellationToken);
                var result = await context.SaveChangesAsync(cancellationToken) > 0;
                if (!result) return Result<Unit>.Failure("Failed to create restaurant");
                return Result<Unit>.Success(Unit.Value);
            }
        }
    }

}
