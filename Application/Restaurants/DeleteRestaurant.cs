using Application.Core;
using Application.Interfaces;
using Domain.Enum;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Restaurants
{
    public class DeleteRestaurant
    {
        public class Command : IRequest<Result<Unit>>
        {
            public Guid Id { get; set; }
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
                var restaurant = await context.Restaurants.Include(a => a.Users).FirstOrDefaultAsync(a => a.Id == request.Id, cancellationToken: cancellationToken);
                if (restaurant == null) return null;
                if (restaurant.Users.FirstOrDefault(a => a.Role == RoleEnum.Manager).Id != userAccessor.GetUserId()) return Result<Unit>.Failure("You can't edit restaurants made by someone else");
                restaurant.IsDeleted = true;
                var result = await context.SaveChangesAsync(cancellationToken) > 0;
                if (!result) return Result<Unit>.Failure("Failed to delete the restaurant");
                return Result<Unit>.Success(Unit.Value);
            }
        }
    }
}
