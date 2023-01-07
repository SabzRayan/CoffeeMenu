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

namespace Application.Branches
{
    public class CreateBranch
    {
        public class Command : IRequest<Result<Unit>>
        {
            public Branch Branch { get; set; }
        }

        public class CommandValidator : AbstractValidator<Command>
        {
            public CommandValidator()
            {
                RuleFor(a => a.Branch).SetValidator(new BranchValidator());
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
                var myRestaurant = await context.Restaurants.FirstOrDefaultAsync(a => a.Users.Any(b => b.Role == RoleEnum.Manager && b.Id == userAccessor.GetUserId()), cancellationToken: cancellationToken);
                if (myRestaurant == null) return Result<Unit>.Failure("You have to save your restaurant data first!");
                request.Branch.RestaurantId = myRestaurant.Id;
                await context.Branches.AddAsync(request.Branch, cancellationToken);
                var result = await context.SaveChangesAsync(cancellationToken) > 0;
                if (!result) return Result<Unit>.Failure("Failed to create branch");
                return Result<Unit>.Success(Unit.Value);
            }
        }
    }

}
