using Application.Core;
using MediatR;
using Persistence;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Modules
{
    public class DeleteModule
    {
        public class Command : IRequest<Result<Unit>>
        {
            public Guid Id { get; set; }
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
                var module = await context.Modules.FindAsync(new object[] { request.Id }, cancellationToken: cancellationToken);
                if (module == null) return null;
                module.IsDeleted = true;
                var result = await context.SaveChangesAsync(cancellationToken) > 0;
                if (!result) return Result<Unit>.Failure("Failed to delete the module");
                return Result<Unit>.Success(Unit.Value);
            }
        }
    }
}
