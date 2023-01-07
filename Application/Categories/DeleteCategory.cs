using Application.Core;
using Application.Interfaces;
using MediatR;
using Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Categories
{
    public class DeleteCategory
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
                var category = await context.Categories.FindAsync(new object[] { request.Id }, cancellationToken: cancellationToken);
                if (category == null) return null;
                var me = await context.Users.FindAsync(new object[] { userAccessor.GetUserId() }, cancellationToken);
                if (category.RestaurantId != me.RestaurantId) return Result<Unit>.Failure("You can't delete categories made by someone else");
                category.IsDeleted = true;
                var result = await context.SaveChangesAsync(cancellationToken) > 0;
                if (!result) return Result<Unit>.Failure("Failed to delete the category");
                return Result<Unit>.Success(Unit.Value);
            }
        }
    }
}
