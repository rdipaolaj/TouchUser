using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using user.common.Responses;
using user.data.Repository.Interfaces;
using user.entities;

namespace user.data.Repository;
public class UserValidationRepository : IUserValidationRepository
{
    private readonly ApplicationDbContext _context;
    private readonly ILogger<UserValidationRepository> _logger;

    public UserValidationRepository(ApplicationDbContext context, ILogger<UserValidationRepository> logger)
    {
        _context = context;
        _logger = logger;
    }
    public async Task<ApiResponse<string>> ValidateUserAsync(User user, CancellationToken cancellationToken)
    {
        try
        {
            _logger.LogInformation("ValidateUserAsync iniciado");
            var validationErrors = new List<ErrorDetail>();
            if (await _context.Users.AnyAsync(u => u.Username == user.Username, cancellationToken))
            {
                validationErrors.Add(new ErrorDetail { Code = "USER0009", Description = "El usuario ya existe." });
            }
            if (await _context.Users.AnyAsync(u => u.Email == user.Email, cancellationToken))
            {
                validationErrors.Add(new ErrorDetail { Code = "USER0010", Description = "El correo ya existe." });
            }
            if (await _context.Users.AnyAsync(u => u.PhoneNumber == user.PhoneNumber, cancellationToken))
            {
                validationErrors.Add(new ErrorDetail { Code = "USER0011", Description = "El número de teléfono ya existe." });

            }
            if (validationErrors.Any())
            {
                return ApiResponseHelper.CreateValidationErrorResponse<string>(validationErrors);
            }
            return ApiResponseHelper.CreateSuccessResponse("Usuario validado.");
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error en ValidateUserAsync: {ex.Message}");
            return ApiResponseHelper.CreateErrorResponse<string>("Error al validar el usuario.");
        }
        
    }
}
