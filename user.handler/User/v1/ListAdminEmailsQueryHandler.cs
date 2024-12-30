using MediatR;
using Microsoft.Extensions.Logging;
using user.application.User;
using user.common.Responses;
using user.request.Querys.v1.User;

namespace user.handler.User.v1;
public class ListAdminEmailsQueryHandler : IRequestHandler<ListAdminEmailsQuery, ApiResponse<IEnumerable<string>>>
{
    private readonly ILogger<ListAdminEmailsQueryHandler> _logger;
    private readonly IUserService _userService;

    public ListAdminEmailsQueryHandler(ILogger<ListAdminEmailsQueryHandler> logger, IUserService userService)
    {
        _logger = logger;
        _userService = userService;
    }

    public async Task<ApiResponse<IEnumerable<string>>> Handle(ListAdminEmailsQuery request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Fetching admin emails...");

        try
        {
            var adminEmails = await _userService.GetAdminEmailsAsync(cancellationToken);

            if (!adminEmails.Any())
            {
                return ApiResponseHelper.CreateSuccessResponse<IEnumerable<string>>(
                    adminEmails, "No se encontraron usuarios administradores.");
            }

            return ApiResponseHelper.CreateSuccessResponse(adminEmails, "Lista de correos electrónicos obtenida con éxito.");
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error fetching admin emails: {ex.Message}");
            return ApiResponseHelper.CreateErrorResponse<IEnumerable<string>>(
                "Ocurrió un error al obtener los correos electrónicos de los administradores.");
        }
    }
}
