using Application.Interfaces;
using Infrastructure.Configuration;
using Infrastructure.Data;
using Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.Extensions;

/// <summary>
/// Provides extension methods for configuring infrastructure services.
/// </summary>
public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Adds infrastructure services to the service collection.
    /// </summary>
    /// <param name="services">The service collection.</param>
    /// <param name="configuration">The configuration.</param>
    /// <returns>The service collection for chaining.</returns>
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        // Configure connection strings options
        var connectionStringsOptions = configuration
            .GetSection(ConnectionStringsOptions.SectionName)
            .Get<ConnectionStringsOptions>() ?? new ConnectionStringsOptions();

        services.Configure<ConnectionStringsOptions>(
            configuration.GetSection(ConnectionStringsOptions.SectionName));

        // Add DbContext with the appropriate provider
        services.AddDbContext<ApplicationDbContext>(options =>
        {
            var provider = connectionStringsOptions.Provider.ToLowerInvariant();
            var connectionString = connectionStringsOptions.DefaultConnection;

            switch (provider)
            {
                case "sqlserver":
                    options.UseSqlServer(connectionString, sqlOptions =>
                    {
                        sqlOptions.EnableRetryOnFailure(
                            maxRetryCount: 5,
                            maxRetryDelay: TimeSpan.FromSeconds(30),
                            errorNumbersToAdd: null);
                    });
                    break;

                case "sqlite":
                default:
                    options.UseSqlite(connectionString);
                    break;
            }

            // Enable sensitive data logging in development
            options.EnableSensitiveDataLogging(false);
            options.EnableDetailedErrors(true);
        });

        // Register repositories
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IExerciseRepository, ExerciseRepository>();
        services.AddScoped<IWorkoutRepository, WorkoutRepository>();

        // Register generic repository
        services.AddScoped(typeof(IRepository<>), typeof(Repository<>));

        return services;
    }

    /// <summary>
    /// Adds Entity Framework Core health checks to the service collection.
    /// </summary>
    /// <param name="services">The service collection.</param>
    /// <returns>The service collection for chaining.</returns>
    public static IServiceCollection AddDatabaseHealthChecks(this IServiceCollection services)
    {
        services.AddHealthChecks()
            .AddDbContextCheck<ApplicationDbContext>(
                name: "database",
                tags: new[] { "db", "ready" });

        return services;
    }
}
