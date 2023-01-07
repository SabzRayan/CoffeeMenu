using Application.Core;
using Application.Interfaces;
using Domain;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Products
{
    public class EditProduct
    {
        public class Command : IRequest<Result<Unit>>
        {
            public Product Product { get; set; }
        }

        public class CommandValidator : AbstractValidator<Command>
        {
            public CommandValidator()
            {
                RuleFor(x => x.Product).SetValidator(new ProductValidator());
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
                var product = await context.Products.Include(a => a.Attachments).Include(a => a.Category).FirstOrDefaultAsync(a => a.Id == request.Product.Id, cancellationToken: cancellationToken);
                if (product == null) return null;
                var me = await context.Users.FindAsync(new object[] { userAccessor.GetUserId() }, cancellationToken);
                if (me.RestaurantId != product.Category.RestaurantId) return Result<Unit>.Failure("You can't edit products from a restaurant made by someone else");
                product.Calory = request.Product.Calory;
                product.CategoryId = request.Product.CategoryId;
                product.Description = request.Product.Description;
                product.Discount = request.Product.Discount;
                product.IsAvailable = request.Product.IsAvailable;
                product.IsExist = request.Product.IsExist;
                product.Price = request.Product.Price;
                product.Recipe = request.Product.Recipe;
                product.Tags = request.Product.Tags;
                product.Title = request.Product.Title;
                context.RemoveRange(product.Attachments);
                product.Attachments = request.Product.Attachments;
                var result = await context.SaveChangesAsync(cancellationToken) > 0;
                if (!result) return Result<Unit>.Failure("Failed to update product");
                return Result<Unit>.Success(Unit.Value);
            }
        }
    }
}
