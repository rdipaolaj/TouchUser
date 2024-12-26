using Microsoft.Extensions.DependencyInjection;
using user.redis.Services;

namespace user.redis;

/// <summary>
/// Métodos de extensión para configuración de redis
/// </summary>
public static class RedisServiceConfiguration
{
    /// <summary>
    /// Configuración de servicio redis
    /// </summary>
    /// <param name="services"></param>
    /// <returns>Retorna service collection para que funcione como método de extensión</returns>
    public static IServiceCollection AddRedisServiceConfiguration(this IServiceCollection services)
    {
        services.AddSingleton<IRedisService, RedisService>();

        return services;
    }
}
