using API.Extensions;
using API.Middleware;
using API.SignalR;
using Application.Cities;
using Domain;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Persistence;
using System;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers(opt =>
{
    var policy = new AuthorizationPolicyBuilder().RequireAuthenticatedUser().Build();
    opt.Filters.Add(new AuthorizeFilter(policy));
});

builder.Services.AddApplicationServices(builder.Configuration);
builder.Services.AddIdentityServices(builder.Configuration);

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseMiddleware<ExceptionMiddleware>();

//app.UseXContentTypeOptions();
//app.UseReferrerPolicy(opt => opt.NoReferrer());
//app.UseXXssProtection(opt => opt.EnabledWithBlockMode());
//app.UseXfo(opt => opt.Deny());
//app.UseCsp(opt => opt
//    .BlockAllMixedContent()
//    .StyleSources(s => s.Self().CustomSources("https://fonts.googleapis.com"))
//    .FontSources(s => s.Self().CustomSources("https://fonts.gstatic.com", "data:"))
//    .FormActions(s => s.Self())
//    .FrameAncestors(s => s.Self())
//    .ImageSources(s => s.Self().CustomSources("https://res.cloudinary.com", "blob:"))
//    .ScriptSources(s => s.Self().CustomSources("sha256-47DEQpj8HBSa+/TImW+5JCeuQeRkm5NMpJWZG3hSuFU=", "sha256-tVFibyLEbUGj+pO/ZSi96c01jJCvzWilvI5Th+wLeGE="))
//);

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "WebAPIv7 v1"));
}
else
{
    app.UseHsts();
    //app.Use(async (context, next) =>
    //{
    //    context.Response.Headers.Add("Strict-Transport-Security", "max-age=31536000");
    //    await next.Invoke();
    //});
}

//app.UseHttpsRedirection();

app.UseDefaultFiles();
app.UseStaticFiles();

app.UseCors("CorsPolicy");

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.MapHub<ChatHub>("/chat");
app.MapFallbackToController("Index", "Fallback");

using var scope = app.Services.CreateScope();
var services = scope.ServiceProvider;
try
{
    var context = services.GetRequiredService<DataContext>();
    var userManager = services.GetRequiredService<UserManager<User>>();
    var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();
    await context.Database.MigrateAsync();
    await Seed.SeedData(context, userManager, roleManager);
}
catch (Exception ex)
{
    var logger = services.GetRequiredService<ILogger<Program>>();
    logger.LogError(ex, "An error occured during migration");
}

await app.RunAsync();
