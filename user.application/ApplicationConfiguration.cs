﻿using Microsoft.Extensions.DependencyInjection;
using user.application.Rol;
using user.application.User;

namespace user.application;
public static class ApplicationConfiguration
{
    public static IServiceCollection AddApplicationConfiguration(this IServiceCollection services)
    {
        services.AddTransient<IRoleValidationService, RoleValidationService>();
        services.AddTransient<IUserService, UserService>();
        services.AddTransient<IUserValidationService, UserValidationService>();
        return services;
    }
}
