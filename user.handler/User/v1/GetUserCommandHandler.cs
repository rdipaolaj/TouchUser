using MediatR;
using Microsoft.Extensions.Logging;
using user.application.User;
using user.common.Responses;
using user.dto.User.v1;
using user.internalservices.Helpers;
using user.request.Commands.v1;

namespace user.handler.User.v1;

public class GetUserCommandHandler : IRequestHandler<GetUserCommand, ApiResponse<GetUserResponse>>
{
    private readonly ILogger<GetUserCommandHandler> _logger;
    private readonly IPasswordHasher _passwordHasher;
    private readonly IUserService _userService;
    public GetUserCommandHandler(
        ILogger<GetUserCommandHandler> logger,
        IPasswordHasher passwordHasher,
        IUserService userService)
    {
        _logger = logger;
        _passwordHasher = passwordHasher;
        _userService = userService;
    }
    public async Task<ApiResponse<GetUserResponse>> Handle(GetUserCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Iniciando GetUserCommandHandler...");
        var userEntity = await _userService.GetUserByUsernameAsync(request.Username, cancellationToken);

        if (userEntity is null)
        {
            _logger.LogWarning("Usuario no encontrado: {Username}", request.Username);
            return ApiResponseHelper.CreateErrorResponse<GetUserResponse>(message: "Invalid credentials", statusCode: 401);
        }
        var isPasswordCorrect = _passwordHasher.VerifyPassword(plainPassword: request.Password, storedHash: userEntity.HashedPassword, storedSalt: userEntity.Salt);

        if (!isPasswordCorrect)
        {
            _logger.LogWarning("Contraseña incorrecta para usuario: {Username}", request.Username);
            return ApiResponseHelper.CreateErrorResponse<GetUserResponse>(message: "Invalid credentials", statusCode: 401);
        }
        var response = new GetUserResponse
        {
            UserId = userEntity.UserId,
            Username = userEntity.Username,
            Email = userEntity.Email,
            PhoneNumber = userEntity.PhoneNumber,
            CreatedAt = userEntity.CreatedAt,
            UpdatedAt = userEntity.UpdatedAt,
            AccountStatus = userEntity.AccountStatus,
            RoleId = userEntity.RoleId
        };

        _logger.LogInformation("Usuario validado correctamente: {UserId}", userEntity.UserId);
        return ApiResponseHelper.CreateSuccessResponse(response, "User found successfully");
    }
}
