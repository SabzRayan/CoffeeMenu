using Application.Core;
using AutoMapper;
using Domain;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Categories
{
    public class EditCategory
    {
        public class Command : IRequest<Result<Unit>>
        {
            public Category Category { get; set; }
        }

        public class CommandValidator : AbstractValidator<Command>
        {
            public CommandValidator()
            {
                RuleFor(x => x.Category).SetValidator(new CategoryValidator());
            }
        }

        public class Handler : IRequestHandler<Command, Result<Unit>>
        {
            private readonly DataContext context;

            public Handler(DataContext context)
            {
                this.context = context;
            }

            public async Task<Result<Unit>> Handle(Command request, CancellationToken cancellationToken)
            {
                var category = await context.Categories.Include(a => a.Attachments).FirstOrDefaultAsync(a => a.Id == request.Category.Id, cancellationToken: cancellationToken);
                if (category == null) return null;
                category.Name = request.Category.Name;
                context.RemoveRange(category.Attachments);
                category.Attachments = request.Category.Attachments;
                var result = await context.SaveChangesAsync(cancellationToken) > 0;
                if (!result) return Result<Unit>.Failure("Failed to update category");
                return Result<Unit>.Success(Unit.Value);
            }
        }
    }
}
