using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using user.internalservices.Helpers;

namespace user.internalservices;
public static class InternalServicesConfiguration
{
    public static IServiceCollection AddInternalServicesConfiguration(this IServiceCollection services)
    {
        //services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
        services.AddTransient<IPasswordHasher, Argon2PasswordHasher>();
        return services;
    }
}