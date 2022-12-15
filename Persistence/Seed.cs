using Domain;
using Domain.Constant;
using Domain.Enum;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Persistence
{
    public class Seed
    {
        public static async Task SeedData(DataContext context, UserManager<User> userManager, RoleManager<IdentityRole> roleManager)
        {
            if (!await userManager.Users.AnyAsync())
            {
                var user = new User
                {
                    Email = "admin@coffeemenu.ir",
                    EmailConfirmed = true,
                    PhoneNumber = "+989117762926",
                    PhoneNumberConfirmed = true,
                    UserName = "Coffee Menu"
                };

                var adminRole = new IdentityRole { Name = Constants.AdminRole };
                var managerRole = new IdentityRole { Name = Constants.ManagerRole };
                var cashierRole = new IdentityRole { Name = Constants.CashierRole };
                var waiterRole = new IdentityRole { Name = Constants.WaiterRole };
                await roleManager.CreateAsync(adminRole);
                await roleManager.CreateAsync(managerRole);
                await roleManager.CreateAsync(cashierRole);
                await roleManager.CreateAsync(waiterRole);

                await context.Modules.AddRangeAsync(new List<Module> {
                    new Module
                    {
                        Name = "Remove Ad",
                        Description = "Removing Ads from your page",
                        Price = 500000
                    },
                    new Module
                    {
                        Name = "Using your Domain",
                        Description = "Show your menu on your own domain address",
                        Price = 500000
                    }
                });

                await userManager.CreateAsync(user, "Pa$$w0rd");
                await userManager.AddToRoleAsync(user, Constants.AdminRole);
                await context.SaveChangesAsync();
            }
        }
    }
}
