using Domain.Enum;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Security
{
    public class IsManagerRequirement : IAuthorizationRequirement
    {
    }

    public class IsManagerRequirementHandler : AuthorizationHandler<IsManagerRequirement>
    {
        private readonly DataContext dbContext;

        public IsManagerRequirementHandler(DataContext dbContext)
        {
            this.dbContext = dbContext;
        }

        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, IsManagerRequirement requirement)
        {
            var userId = context.User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null) return Task.CompletedTask;
            var user = dbContext.Users.AsNoTracking().FirstOrDefaultAsync(a => a.Id == userId).Result;
            if (user.Role == RoleEnum.Manager) context.Succeed(requirement);
            return Task.CompletedTask;
        }
    }
}
