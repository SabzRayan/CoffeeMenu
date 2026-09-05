using Application.Core;
using Application.Interfaces;
using AutoMapper;
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
    public class EditUser
    {
        public class Command : IRequest<Result<Unit>>
        {
            public User User { get; set; }
        }

        public class CommandValidator : AbstractValidator<Command>
        {
            public CommandValidator()
            {
                RuleFor(x => x.User).SetValidator(new UserValidator());
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
                var user = await context.Users.FirstOrDefaultAsync(a => a.Id == request.User.Id, cancellationToken: cancellationToken);
                if (user == null) return null;
                if (user.RestaurantId != myRestaurant.Id) return Result<Unit>.Failure("You can't delete users made by someone else");
                if (!myRestaurant.Branches.Any(a => a.Id == request.User.BranchId)) return Result<Unit>.Failure("You can't save user to the branches that not belongs to you!");
                if (request.User.Role != RoleEnum.Cashier && request.User.Role != RoleEnum.Waiter) return Result<Unit>.Failure("You can't save manager or admin user");
                user.Name = request.User.Name;
                if (user.Role != RoleEnum.Manager)
                {
                    user.BranchId = request.User.BranchId;
                    user.Role = request.User.Role;
                }
                var result = await context.SaveChangesAsync(cancellationToken) > 0;
                if (!result) return Result<Unit>.Failure("Failed to update user");
                return Result<Unit>.Success(Unit.Value);
            }
        }
    }
}
