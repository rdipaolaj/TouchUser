using Asp.Versioning;
using Mapster;
using MapsterMapper;
using Microsoft.AspNetCore.Antiforgery;
using Microsoft.Extensions.Options;
using Swashbuckle.AspNetCore.SwaggerGen;
using user.api.Configuration.Security;
using user.api.Configuration;
using user.common.Secrets;
using user.common.Settings;
using user.data.HealthCheck;
using user.dto.Mapster;
using user.secretsmanager.Service;
using Microsoft.AspNetCore.Mvc;
using user.request.Mapster;
using user.application.Mapster;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using user.common.Responses;
using System.Text.Json;

namespace user.api;

public static class ProgramExtesions
{
    public static IServiceCollection AddCustomMvc(this IServiceCollection services, WebApplicationBuilder builder)
    {
        string[] domains = builder.Configuration.GetSection("CorsDomains").Get<string[]>();

        services.AddControllersWithViews();

        services.AddCors(options =>
        {
            options.AddPolicy("CorsPolicy",
                builder => builder
                .SetIsOriginAllowed((host) => true)
                .WithOrigins(domains)
                .AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader());
        });

        services.AddOptions();

        services.Configure<ApiBehaviorOptions>(options =>
        {
            options.SuppressModelStateInvalidFilter = true;
        });

        return services;
    }

    public static IServiceCollection AddConfigurationSettings(this IServiceCollection services, WebApplicationBuilder builder)
    {
        builder.Services.Configure<RedisSettings>(builder.Configuration.GetSection("RedisSettings"));
        builder.Services.Configure<SecretManagerSettings>(builder.Configuration.GetSection("SecretManagerSettings"));
        builder.Services.Configure<JwtSettings>(builder.Configuration.GetSection("JwtSettings"));

        return services;
    }

    public static IServiceCollection AddSecretsConfiguration(this IServiceCollection services, WebApplicationBuilder builder)
    {
        ServiceProvider serviceProvider = services.BuildServiceProvider();
        var secretManagerService = (ISecretManagerService)serviceProvider.GetService(typeof(ISecretManagerService));

        PostgresDbSecrets secretsPostgres = secretManagerService.GetPostgresDbSecrets().GetAwaiter().GetResult();
        RedisSecrets redisSecrets = secretManagerService.GetRedisSecrets().GetAwaiter().GetResult();
        JwtSecrets secretsAuth = secretManagerService.GetJwtSecrets().GetAwaiter().GetResult();

        services.Configure<PostgresDbSettings>(options =>
        {
            options.Username = secretsPostgres.Username;
            options.Password = secretsPostgres.Password;
            options.Host = secretsPostgres.Host;
            options.Port = secretsPostgres.Port;
            options.Dbname = secretsPostgres.Dbname;
        });

        services.Configure<RedisKeySettings>(options =>
        {
            options.PrivateKey = redisSecrets.PrivateKey;
        });

        services.Configure<JwtSettings>(options =>
        {
            options.Secret = secretsAuth.JwtSigningKey;
        });

        return services;
    }

    public static IServiceCollection AddMapsterConfiguration(this IServiceCollection services)
    {
        TypeAdapterConfig mapsterConfiguration = MapsterConfiguration.Configuration();
        TypeAdapterConfig mapsterRequestConfiguration = MapsterRequestConfiguration.Configuration();
        TypeAdapterConfig mapsterApplicationConfiguration = MapsterApplicationConfiguration.Configuration();

        services.AddSingleton(mapsterConfiguration);
        services.AddSingleton(mapsterRequestConfiguration);
        services.AddSingleton(mapsterApplicationConfiguration);
        services.AddScoped<IMapper, ServiceMapper>();

        return services;
    }

    public static IServiceCollection AddApiVersioningConfiguration(this IServiceCollection services)
    {
        services.AddApiVersioning(options =>
        {
            options.DefaultApiVersion = new ApiVersion(1);
            options.ReportApiVersions = true;
            options.AssumeDefaultVersionWhenUnspecified = true;
            options.ApiVersionReader = ApiVersionReader.Combine(
                new UrlSegmentApiVersionReader(),
                new HeaderApiVersionReader("X-Api-Version"));
        }).AddApiExplorer(options =>
        {
            options.GroupNameFormat = "'v'V";
            options.SubstituteApiVersionInUrl = true;
        });

        services.AddTransient<IConfigureOptions<SwaggerGenOptions>, ConfigureSwaggerOptions>();

        return services;
    }

    public static WebApplication ConfigurationSwagger(this WebApplication app)
    {
        app.UseSwaggerUI(options =>
        {
            var descriptions = app.DescribeApiVersions();

            foreach (var description in descriptions)
            {
                options.SwaggerEndpoint($"/swagger/{description.GroupName}/swagger.json", description.GroupName.ToUpperInvariant());
            }

        });

        return app;
    }

    public static IServiceCollection AddDatabaseHealthCheck(this IServiceCollection services)
    {
        services.AddHealthChecks()
            .AddCheck<PostgresHealthCheck>("PostgreSQL");

        return services;
    }

    public static WebApplication AddSecurityHeaders(this WebApplication app)
    {
        app.UseMiddleware<SecurityHeadersMiddleware>(new SecurityHeadersBuilder().AddDefaultSecurePolicy().Build());
        return app;
    }

    public static IServiceCollection AddAntiForgeryToken(this IServiceCollection services)
    {
        services.AddSingleton<IAntiforgeryAdditionalDataProvider, CustomAntiforgeryDataProvider>();

        services.AddAntiforgery(options =>
        {
            options.HeaderName = "X-CSRF-TOKEN";
        });

        return services;
    }

    public static IServiceCollection AddCustomJwtAuthentication(this IServiceCollection services, IConfiguration configuration)
    {
        var serviceProvider = services.BuildServiceProvider();
        var jwtSettings = serviceProvider.GetRequiredService<IOptions<JwtSettings>>().Value;

        if (jwtSettings == null || string.IsNullOrEmpty(jwtSettings.Secret))
        {
            throw new InvalidOperationException("La configuración de JWT no está correctamente configurada en appsettings.json.");
        }

        var secretKey = Convert.FromBase64String(jwtSettings.Secret);

        services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        })
        .AddJwtBearer(options =>
        {
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = jwtSettings.Issuer,
                ValidAudience = jwtSettings.Audience,
                IssuerSigningKey = new SymmetricSecurityKey(secretKey),
                ClockSkew = TimeSpan.Zero
            };

            options.Events = new JwtBearerEvents
            {
                OnChallenge = async context =>
                {
                    context.HandleResponse();
                    context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                    context.Response.ContentType = "application/json";

                    var response = new ApiResponse<object>
                    {
                        Success = false,
                        StatusCode = 401,
                        Message = "Falta el encabezado de autorización o el token no es válido.",
                        Errors = new List<ErrorDetail>
                        {
                        new ErrorDetail
                        {
                            Code = "AUTH001",
                            Description = "Token ausente o no válido en la solicitud."
                        }
                        }
                    };

                    await context.Response.WriteAsync(JsonSerializer.Serialize(response));
                },
                OnAuthenticationFailed = async context =>
                {
                    context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                    context.Response.ContentType = "application/json";

                    var response = new ApiResponse<object>
                    {
                        Success = false,
                        StatusCode = 401,
                        Message = "La autenticación falló.",
                        Errors = new List<ErrorDetail>
                        {
                        new ErrorDetail
                        {
                            Code = "AUTH002",
                            Description = context.Exception.Message
                        }
                        }
                    };

                    await context.Response.WriteAsync(JsonSerializer.Serialize(response));
                }
            };
        });

        return services;
    }
}
