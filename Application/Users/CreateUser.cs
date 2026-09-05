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

namespace Application.Users
{
    public class CreateUser
    {
        public class Command : IRequest<Result<Unit>>
        {
            public User User { get; set; }
        }

        public class CommandValidator : AbstractValidator<Command>
        {
            public CommandValidator()
            {
                RuleFor(a => a.User).SetValidator(new UserValidator());
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
                var myRestaurant = await context.Restaurants.Include(a => a.Branches).FirstOrDefaultAsync(a => a.Users.Any(b => b.Role == RoleEnum.Manager && b.Id == userAccessor.GetUserId()), cancellationToken: cancellationToken);
                if (myRestaurant == null) return Result<Unit>.Failure("You have to save your restaurant data first!");
                if (!myRestaurant.Branches.Any(a => a.Id == request.User.BranchId)) return Result<Unit>.Failure("You can't save user to the branches that not belongs to you!");
                if (request.User.Role != RoleEnum.Cashier && request.User.Role != RoleEnum.Waiter) return Result<Unit>.Failure("You can't save manager or admin user");
                request.User.RestaurantId = myRestaurant.Id;
                await context.Users.AddAsync(request.User, cancellationToken);
                var result = await context.SaveChangesAsync(cancellationToken) > 0;
                if (!result) return Result<Unit>.Failure("Failed to create user");
                return Result<Unit>.Success(Unit.Value);
            }
        }
    }

}
