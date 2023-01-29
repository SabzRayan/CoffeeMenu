using Application.Core;
using Application.Interfaces;
using Domain;
using FluentValidation;
using MediatR;
using Persistence;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Products
{
    public class CreateProduct
    {
        public class Command : IRequest<Result<Unit>>
        {
            public Product Product { get; set; }
        }

        public class CommandValidator : AbstractValidator<Command>
        {
            public CommandValidator()
            {
                RuleFor(a => a.Product).SetValidator(new ProductValidator());
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
                var me = await context.Users.FindAsync(new object[] { userAccessor.GetUserId() }, cancellationToken);
                var category = await context.Categories.FindAsync(new object[] { request.Product.CategoryId }, cancellationToken: cancellationToken);
                if (me.RestaurantId != category.RestaurantId) return Result<Unit>.Failure("You can't add products to a restaurant made by someone else");
                await context.Products.AddAsync(request.Product, cancellationToken);
                var result = await context.SaveChangesAsync(cancellationToken) > 0;
                if (!result) return Result<Unit>.Failure("Failed to create product");
                return Result<Unit>.Success(Unit.Value);
            }
        }
    }

}
