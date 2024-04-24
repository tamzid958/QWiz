using Microsoft.EntityFrameworkCore;
using QWiz.Databases;
using QWiz.Repositories.Wrapper;

namespace QWiz.Helpers.Extensions;

public static class DbExtension
{
    public static void ConfigureDbConnection(this IServiceCollection services, IConfiguration config)
    {
        services.AddDbContext<AppDbContext>(o =>
            o.UseSqlServer(config.GetConnectionString("AppDbContextConnection")));

        services.AddScoped<IRepositoryWrapper, RepositoryWrapper>();
    }

    public static void EnsureMigration(this WebApplication? app)
    {
        using var scope = app!.Services.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        if (db.Database.GetPendingMigrations().Any()) db.Database.Migrate();
    }
}