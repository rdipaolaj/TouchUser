using Mapster;
using user.common.Constant;
using user.request.Commands.v1;

namespace user.application.Mapster;
public static class MapsterApplicationConfiguration
{
    public static TypeAdapterConfig Configuration()
    {
        TypeAdapterConfig config = new();

        // Configuración de CreateUserCommand a User
        config.NewConfig<CreateUserCommand, user.entities.User>()
            .Map(dest => dest.UserId, src => Guid.NewGuid())
            .Map(dest => dest.CreatedAt, src => DateTime.UtcNow)
            .Map(dest => dest.AccountStatus, src => CommonConstant.Active);

        return config;
    }
}