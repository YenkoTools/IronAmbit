using Application.Behaviors;
using Application.Common;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using FluentValidation;

namespace Application.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        // Register dispatchers
        services.AddTransient<ICommandDispatcher, CommandDispatcher>();
        services.AddTransient<IQueryDispatcher, QueryDispatcher>();

        // Register services
        services.AddSingleton<IMetricsService, MetricsService>();

        // Register pipeline behaviors (order matters - validation first, then performance, then metrics)
        services.AddScoped(typeof(IQueryPipelineBehavior<,>), typeof(QueryValidationBehavior<,>));
        services.AddScoped(typeof(ICommandPipelineBehavior<,>), typeof(CommandValidationBehavior<,>));
        services.AddScoped(typeof(IQueryPipelineBehavior<,>), typeof(QueryPerformanceBehavior<,>));
        services.AddScoped(typeof(ICommandPipelineBehavior<,>), typeof(CommandPerformanceBehavior<,>));
        services.AddScoped(typeof(ICommandPipelineBehavior<,>), typeof(CommandMetricsBehavior<,>));
        services.AddScoped(typeof(IQueryPipelineBehavior<,>), typeof(QueryMetricsBehavior<,>));

        // Auto-register all handlers and validators from the Application assembly
        var applicationAssembly = typeof(ServiceCollectionExtensions).Assembly;

        RegisterHandlers(services, applicationAssembly);
        RegisterValidators(services, applicationAssembly);

        return services;
    }

    private static void RegisterHandlers(IServiceCollection services, Assembly assembly)
    {
        // Register both IQueryHandler<,> and ICommandHandler<,> implementations
        var handlerTypes = new[] { typeof(IQueryHandler<,>), typeof(ICommandHandler<,>) };

        foreach (var handlerType in handlerTypes)
        {
            var handlers = assembly.GetTypes()
                .Where(t => t.IsClass && !t.IsAbstract)
                .Select(t => new
                {
                    Type = t,
                    Interfaces = t.GetInterfaces()
                        .Where(i => i.IsGenericType && i.GetGenericTypeDefinition() == handlerType)
                        .ToList()
                })
                .Where(x => x.Interfaces.Any())
                .ToList();

            foreach (var handler in handlers)
            {
                foreach (var @interface in handler.Interfaces)
                {
                    services.AddScoped(@interface, handler.Type);
                }
            }
        }
    }

    private static void RegisterValidators(IServiceCollection services, Assembly assembly)
    {
        var validatorType = typeof(IValidator<>);

        var validators = assembly.GetTypes()
            .Where(t => t.IsClass && !t.IsAbstract)
            .Select(t => new
            {
                Type = t,
                Interfaces = t.GetInterfaces()
                    .Where(i => i.IsGenericType && i.GetGenericTypeDefinition() == validatorType)
                    .ToList()
            })
            .Where(x => x.Interfaces.Any())
            .ToList();

        foreach (var validator in validators)
        {
            foreach (var @interface in validator.Interfaces)
            {
                services.AddScoped(@interface, validator.Type);
            }
        }
    }
}
