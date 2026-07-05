using Microsoft.AspNetCore.Identity;

namespace Samples.SSR.GUI;

public static class AppComposition
{
    public static void Configure(WebApplicationBuilder builder, DatabaseCredentials databaseCredentials)
    {
        DatabaseConfig.AddDatabase(builder.Services, databaseCredentials);

        builder.Services
            .AddIdentity<IdentityUser, IdentityRole>(options =>
            {
                options.Password.RequireDigit = true;
                options.Password.RequireUppercase = true;
                options.Password.RequireNonAlphanumeric = true;
            })
            .AddEntityFrameworkStores<AuthDbContext>()
            .AddDefaultTokenProviders();

        builder.Services.ConfigureApplicationCookie(options =>
        {
            options.Events.OnRedirectToLogin = ctx =>
            {
                ctx.Response.StatusCode = 401;
                return Task.CompletedTask;
            };
            options.Events.OnRedirectToAccessDenied = ctx =>
            {
                ctx.Response.StatusCode = 403;
                return Task.CompletedTask;
            };
        });

        builder.Services.AddControllersWithViews();
    }
}
