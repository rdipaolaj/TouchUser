using Microsoft.Extensions.DependencyInjection;
using user.infraestructure.Behaviors;
using user.internalservices;
using user.redis;
using user.secretsmanager;

namespace user.infraestructure.Modules;
public static class MediatorModule
{
    public static IServiceCollection AddMediatRAssemblyConfiguration(this IServiceCollection services)
    {
        //services.AddMediatR(configuration =>
        //{

        //    configuration.AddOpenBehavior(typeof(ValidatorBehavior<,>));
        //});

        //services.AddValidatorsFromAssembly(typeof(CreateUserCommandValidator).Assembly);

        return services;
    }
    public static IServiceCollection AddCustomServicesConfiguration(this IServiceCollection services)
    {
        services.AddInternalServicesConfiguration();
        services.AddSecretManagerConfiguration();
        services.AddRedisServiceConfiguration();

        return services;
    }
}