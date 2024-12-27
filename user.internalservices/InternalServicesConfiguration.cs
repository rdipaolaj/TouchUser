using Microsoft.Extensions.DependencyInjection;
using user.internalservices.Helpers;

namespace user.internalservices;
public static class InternalServicesConfiguration
{
    public static IServiceCollection AddInternalServicesConfiguration(this IServiceCollection services)
    {
        services.AddTransient<IPasswordHasher, Argon2PasswordHasher>();
        return services;
    }
}