using Application.Core;
using Domain;
using FluentValidation;
using MediatR;
using Persistence;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Modules
{
    public class EditModule
    {
        public class Command : IRequest<Result<Unit>>
        {
            public Module Module { get; set; }
        }

        public class CommandValidator : AbstractValidator<Command>
        {
            public CommandValidator()
            {
                RuleFor(a => a.Module).SetValidator(new ModuleValidator());
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
                var module = await context.Modules.FindAsync(new object[] { request.Module.Id }, cancellationToken);
                if (module == null) return null;
                module.Description = request.Module.Description;
                module.Name = request.Module.Name;
                module.Price = request.Module.Price;
                var result = await context.SaveChangesAsync(cancellationToken) > 0;
                if (!result) return Result<Unit>.Failure("Failed to update module");
                return Result<Unit>.Success(Unit.Value);
            }
        }
    }
}
