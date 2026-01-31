namespace Samples.SSR.GUI;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        
        var postgresHost = Environment.GetEnvironmentVariable("POSTGRES_HOST") ?? "localhost";
        var postgresPort = Environment.GetEnvironmentVariable("POSTGRES_PORT") ?? "5433";
        var dbUser       = Environment.GetEnvironmentVariable("POSTGRES_USER") ?? "test";
        var dbPassword   = Environment.GetEnvironmentVariable("POSTGRES_PASSWORD") ?? "test";
        var dbName       = Environment.GetEnvironmentVariable("POSTGRES_DB") ?? "testdb";
        var connectionString = $"Host={postgresHost};Port={postgresPort};Username={dbUser};Password={dbPassword};Database={dbName}";

        AppComposition.Configure(builder, new DatabaseCredentials(connectionString));

        var app = builder.Build();

        DatabaseConfig.RunMigrations(app.Services, builder.Environment.ContentRootPath);
        
        app.UseRouting();
        app.UseAuthentication();
        app.UseAuthorization();

        app.MapStaticAssets();
        app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}")
            .WithStaticAssets();

        app.Run();

    }
}