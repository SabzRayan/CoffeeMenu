using Application.Core;
using Application.Interfaces;
using Domain.Enum;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Users
{
    public class DeleteUser
    {
        public class Command : IRequest<Result<Unit>>
        {
            public string Id { get; set; }
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
                var user = await context.Users.FirstOrDefaultAsync(a => a.Id == request.Id, cancellationToken: cancellationToken);
                if (user == null) return null;
                if (user.RestaurantId != myRestaurant.Id) return Result<Unit>.Failure("You can't delete users made by someone else");
                user.IsDeleted = true;
                user.LockoutEnabled = true;
                var result = await context.SaveChangesAsync(cancellationToken) > 0;
                if (!result) return Result<Unit>.Failure("Failed to delete the user");
                return Result<Unit>.Success(Unit.Value);
            }
        }
    }
}
