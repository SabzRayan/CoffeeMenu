using Application.Core;
using Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Products
{
    public class SetExistProduct
    {
        public class Command : IRequest<Result<Unit>>
        {
            public Guid Id { get; set; }
            public bool IsExist { get; set; }
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
                var product = await context.Products.Include(a => a.Category).FirstOrDefaultAsync(a => a.Id == request.Id, cancellationToken: cancellationToken);
                if (product == null) return null;
                var me = await context.Users.FindAsync(new object[] { userAccessor.GetUserId() }, cancellationToken);
                if (me.RestaurantId != product.Category.RestaurantId) return Result<Unit>.Failure("You can't edit products from a restaurant made by someone else");
                product.IsExist = request.IsExist;
                var result = await context.SaveChangesAsync(cancellationToken) > 0;
                if (!result) return Result<Unit>.Failure("Failed to set existance for the product");
                return Result<Unit>.Success(Unit.Value);
            }
        }
    }
}
