using DbUp;
using Microsoft.EntityFrameworkCore;

namespace Samples.SSR.GUI;

public static class DatabaseConfig
{
    public static void AddDatabase(
        IServiceCollection services,
        DatabaseCredentials dbRuntime)
    {
        services.AddSingleton<DatabaseCredentials>(dbRuntime);

        services.AddDbContext<AuthDbContext>((sp, o) =>
        {
            var db = sp.GetRequiredService<DatabaseCredentials>();
            o.UseNpgsql(db.ConnectionString);
        });
    }
    
    public static void RunMigrations(IServiceProvider services, string contentRoot)
    {
        var config = services.GetRequiredService<DatabaseCredentials>();

        var migrationsPath = Path.Combine(contentRoot, "Migrations");

        var upgrader = DeployChanges.To
            .PostgresqlDatabase(config.ConnectionString)
            .WithScriptsFromFileSystem(migrationsPath)
            .Build();

        var result = upgrader.PerformUpgrade();
        if (!result.Successful)
            throw result.Error;
    }
}
