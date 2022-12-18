using Domain.Enum;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Persistence;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Infrastructure.Security
{
    public class IsCashierRequirement : IAuthorizationRequirement
    {
    }

    public class IsCashierRequirementHandler : AuthorizationHandler<IsCashierRequirement>
    {
        private readonly DataContext dbContext;

        public IsCashierRequirementHandler(DataContext dbContext)
        {
            this.dbContext = dbContext;
        }

        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, IsCashierRequirement requirement)
        {
            var userId = context.User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null) return Task.CompletedTask;
            var user = dbContext.Users.AsNoTracking().FirstOrDefaultAsync(a => a.Id == userId).Result;
            if (user.Role == RoleEnum.Cashier) context.Succeed(requirement);
            return Task.CompletedTask;
        }
    }
}
