using Application.Core;
using Application.Interfaces;
using AutoMapper;
using Domain;
using Domain.Enum;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Branches
{
    public class EditBranch
    {
        public class Command : IRequest<Result<Unit>>
        {
            public Branch Branch { get; set; }
        }

        public class CommandValidator : AbstractValidator<Command>
        {
            public CommandValidator()
            {
                RuleFor(x => x.Branch).SetValidator(new BranchValidator());
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
                var branch = await context.Branches.Include(a => a.Restaurant.Users).FirstOrDefaultAsync(a => a.Id == request.Branch.Id, cancellationToken: cancellationToken);
                if (branch == null) return null;
                if (branch.Restaurant.Users.FirstOrDefault(a => a.Role == RoleEnum.Manager).Id != userAccessor.GetUserId()) return Result<Unit>.Failure("You can't edit branches made by someone else");
                branch.Name = request.Branch.Name;
                branch.Phone= request.Branch.Phone;
                branch.TableCount= request.Branch.TableCount;
                branch.Lat= request.Branch.Lat;
                branch.Lng= request.Branch.Lng;
                branch.CityId= request.Branch.CityId;
                branch.ProvinceId= request.Branch.ProvinceId;

                var result = await context.SaveChangesAsync(cancellationToken) > 0;
                if (!result) return Result<Unit>.Failure("Failed to update branch");
                return Result<Unit>.Success(Unit.Value);
            }
        }
    }
}
