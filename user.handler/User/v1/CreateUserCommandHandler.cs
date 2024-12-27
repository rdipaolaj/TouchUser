using MediatR;
using Microsoft.Extensions.Logging;
using user.application.Rol;
using user.application.User;
using user.common.Responses;
using user.dto.User.v1;
using user.internalservices.Helpers;
using user.redis.Users;
using user.request.Commands.v1;

namespace user.handler.User.v1;
public class CreateUserCommandHandler : IRequestHandler<CreateUserCommand, ApiResponse<CreateUserResponse>>
{
    private readonly ILogger<CreateUserCommandHandler> _logger;
    private readonly IPasswordHasher _passwordHasher;
    private readonly IRoleValidationService _roleValidationService;
    private readonly IUserValidationService _userValidationService;
    private readonly IUserService _userService;
    private readonly IRedisUserService _redisUserService;

    public CreateUserCommandHandler(
        ILogger<CreateUserCommandHandler> logger,
        IPasswordHasher passwordHasher,
        IRoleValidationService roleValidationService,
        IUserValidationService userValidationService,
        IUserService userService,
        IRedisUserService redisUserService)
    {
        _logger = logger;
        _passwordHasher = passwordHasher;
        _roleValidationService = roleValidationService;
        _userValidationService = userValidationService;
        _userService = userService;
        _redisUserService = redisUserService;
    }

    public async Task<ApiResponse<CreateUserResponse>> Handle(CreateUserCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Iniciando proceso de creación de usuario...");

        var validationUser = await _userValidationService.ValidateUserAsync(request, cancellationToken);

        if (!validationUser.Success)
        {
            _logger.LogWarning("Error en la validación del usuario: {Errors}", validationUser.Errors);
            return ApiResponseHelper.CreateErrorResponse<CreateUserResponse>("User validation failed", 400, validationUser.Errors.ToList());
        }

        var validationRole = await _roleValidationService.ValidateRoleAsync(request.UserRole, cancellationToken);
        if (!validationRole.Success) 
        {
            _logger.LogWarning("Error en la validación del rol: {Errors}", validationRole.Errors);
            return ApiResponseHelper.CreateErrorResponse<CreateUserResponse>("Role validation failed", 400, validationRole.Errors.ToList());
        }

        var (hashedPassword, salt) = _passwordHasher.HashPassword(request.Password);

        // Crear el usuario
        var user = await _userService.CreateUserAsync(request, salt, hashedPassword, validationRole.Data.RoleId, cancellationToken);

        if (user == null || user.UserId == Guid.Empty)
        {
            _logger.LogError("Error al guardar el usuario en la base de datos.");
            return ApiResponseHelper.CreateErrorResponse<CreateUserResponse>("Failed to create user", 500);
        }

        // Sincronizar con Redis
        await _redisUserService.SyncUserWithRedisAsync(user, user.Username);
        _logger.LogInformation("Usuario creado y guardado en Redis correctamente.");

        var response = new CreateUserResponse
        {
            UserId = user.UserId
        };

        return ApiResponseHelper.CreateSuccessResponse(response, "User created successfully");
    }
}
