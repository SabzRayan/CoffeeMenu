using Application.Core;
using Application.Interfaces;
using Domain.Enum;
using MediatR;
using Persistence;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Branches
{
    public class DeleteBranch
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
                var branch = await context.Branches.FindAsync(new object[] { request.Id }, cancellationToken: cancellationToken);
                if (branch == null) return null;
                if (branch.Restaurant.Users.FirstOrDefault(a => a.Role == RoleEnum.Manager).Id != userAccessor.GetUserId()) return Result<Unit>.Failure("You can't delete branches made by someone else");
                branch.IsDeleted = true;
                var result = await context.SaveChangesAsync(cancellationToken) > 0;
                if (!result) return Result<Unit>.Failure("Failed to delete the branch");
                return Result<Unit>.Success(Unit.Value);
            }
        }
    }
}
