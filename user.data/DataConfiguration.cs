using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using user.common.Settings;
using user.data.Repository.Interfaces;
using user.data.Repository;

namespace user.data;
public static class DataConfiguration
{
    public static IServiceCollection AddDatabaseConfiguration(this IServiceCollection services, IConfiguration configuration)
    {
        var serviceProvider = services.BuildServiceProvider();
        var postgresSettings = serviceProvider.GetService<IOptions<PostgresDbSettings>>()?.Value;

        if (postgresSettings == null)
        {
            throw new InvalidOperationException("PostgresDbSettings not configured properly.");
        }

        var connectionString = $"Host={postgresSettings.Host};Port={postgresSettings.Port};Username={postgresSettings.Username};Password={postgresSettings.Password};Database={postgresSettings.Dbname};";

        services.AddDbContext<ApplicationDbContext>(options =>
            options.UseNpgsql(connectionString, npgsqlOptions =>
            {
                npgsqlOptions.CommandTimeout(30);
                npgsqlOptions.MigrationsHistoryTable("__EFMigrationsHistory", "usersch");
            })
        );

        return services;
    }

    public static IServiceCollection AddDataServicesConfiguration(this IServiceCollection services)
    {
        services.AddScoped<IRoleRepository, RoleRepository>();
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IUserValidationRepository, UserValidationRepository>();

        return services;
    }
}
