using Domain;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Persistence;
using System;
using System.Text;
using System.Threading.Tasks;
using API.Services;
using Microsoft.AspNetCore.Authorization;
using Infrastructure.Security;
using Domain.Constant;

namespace API.Extensions
{
    public static class IdentityServiceExtensions
    {
        public static IServiceCollection AddIdentityServices(this IServiceCollection services, IConfiguration config)
        {
            services.AddIdentityCore<User>(opt =>
            {
                opt.Password.RequireNonAlphanumeric = false;
                opt.SignIn.RequireConfirmedEmail = true;
            })
                .AddEntityFrameworkStores<DataContext>()
                .AddSignInManager<SignInManager<User>>()
                .AddDefaultTokenProviders();

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["TokenKey"]));

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(opt =>
                {
                    opt.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = key,
                        ValidateIssuer = false,
                        ValidateAudience = false,
                        ValidateLifetime = true,
                        ClockSkew = TimeSpan.Zero
                    };
                    opt.Events = new JwtBearerEvents
                    {
                        OnMessageReceived = context =>
                        {
                            var accessToken = context.Request.Query["access_token"];
                            var path = context.HttpContext.Request.Path;
                            if (!string.IsNullOrEmpty(accessToken) && (path.StartsWithSegments("/order")))
                            {
                                context.Token = accessToken;
                            }
                            return Task.CompletedTask;
                        }
                    };
                });

            services.AddAuthorization(opt =>
            {
                opt.AddPolicy(Constants.AdminRole, policy =>
                {
                    policy.Requirements.Add(new IsAdminRequirement());
                });
                opt.AddPolicy(Constants.ManagerRole, policy =>
                {
                    policy.Requirements.Add(new IsManagerRequirement());
                });
                opt.AddPolicy(Constants.CashierRole, policy =>
                {
                    policy.Requirements.Add(new IsCashierRequirement());
                });
                opt.AddPolicy(Constants.WaiterRole, policy =>
                {
                    policy.Requirements.Add(new IsWaiterRequirement());
                });
            });
            services.AddTransient<IAuthorizationHandler, IsAdminRequirementHandler>();
            services.AddTransient<IAuthorizationHandler, IsManagerRequirementHandler>();
            services.AddTransient<IAuthorizationHandler, IsCashierRequirementHandler>();
            services.AddTransient<IAuthorizationHandler, IsWaiterRequirementHandler>();
            services.AddScoped<TokenService>();

            return services;
        }
    }
}
