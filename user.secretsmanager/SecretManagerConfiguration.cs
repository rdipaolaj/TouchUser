using Microsoft.Extensions.DependencyInjection;
using user.secretsmanager.Service;

namespace user.secretsmanager;
public static class SecretManagerConfiguration
{
    public static IServiceCollection AddSecretManagerConfiguration(this IServiceCollection services)
    {
        services.AddSingleton<ISecretManagerService, SecretManagerService>();

        return services;
    }
}