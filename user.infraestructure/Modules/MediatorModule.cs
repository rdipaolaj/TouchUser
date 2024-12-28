using Microsoft.Extensions.DependencyInjection;
using user.application;
using user.infraestructure.Behaviors;
using user.internalservices;
using user.redis;
using user.secretsmanager;
using user.handler.User.v1;
using FluentValidation;
using user.requestvalidator.User;
using user.handler.Rol.v1;

namespace user.infraestructure.Modules;
public static class MediatorModule
{
    public static IServiceCollection AddMediatRAssemblyConfiguration(this IServiceCollection services)
    {
        services.AddMediatR(configuration =>
        {
            configuration.RegisterServicesFromAssemblyContaining(typeof(CreateUserCommandHandler));
            configuration.RegisterServicesFromAssemblyContaining(typeof(GetListRolesQueryHandler));
            configuration.RegisterServicesFromAssemblyContaining(typeof(GetUserCommandHandler));

            configuration.AddOpenBehavior(typeof(ValidatorBehavior<,>));
        });

        services.AddValidatorsFromAssembly(typeof(CreateUserCommandValidator).Assembly);
        services.AddValidatorsFromAssembly(typeof(GetUserCommandValidator).Assembly);

        return services;
    }
    public static IServiceCollection AddCustomServicesConfiguration(this IServiceCollection services)
    {
        services.AddInternalServicesConfiguration();
        services.AddSecretManagerConfiguration();
        services.AddRedisServiceConfiguration();
        services.AddApplicationConfiguration();

        return services;
    }
}