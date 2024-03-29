﻿using Application.Core;
using Application.Interfaces;
using Domain;
using Domain.Enum;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Branches
{
    public class AddBranchCategory
    {
        public class Command : IRequest<Result<Unit>>
        {
            public List<BranchCategory> BranchCategories { get; set; }
        }

        public class CommandValidator : AbstractValidator<Command>
        {
            public CommandValidator()
            {
                RuleFor(x => x.BranchCategories).ForEach(a => a.SetValidator(new BranchCategoryValidator()));
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
                var branch = await context.Branches.Include(a => a.Categories).Include(a => a.Restaurant.Users).FirstOrDefaultAsync(x => x.Id == request.BranchCategories.First().BranchId, cancellationToken: cancellationToken);
                if (branch == null) return null;
                if (branch.Restaurant.Users.FirstOrDefault(a => a.Role == RoleEnum.Manager).Id != userAccessor.GetUserId()) return Result<Unit>.Failure("You can't edit branches made by someone else");
                foreach (var branchCategory in request.BranchCategories)
                {
                    var isExist = branch.Categories.FirstOrDefault(a => a.CategoryId == branchCategory.CategoryId);
                    if (isExist != null)
                        branch.Categories.Remove(isExist);
                    else
                        branch.Categories.Add(new BranchCategory { CategoryId = branchCategory.CategoryId });
                }
                var result = await context.SaveChangesAsync(cancellationToken) > 0;
                if (!result) return Result<Unit>.Failure("Failed to add category");
                return Result<Unit>.Success(Unit.Value);
            }
        }
    }
}
