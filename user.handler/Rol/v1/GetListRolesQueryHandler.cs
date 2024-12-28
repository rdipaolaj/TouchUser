using MediatR;
using Microsoft.Extensions.Logging;
using user.application.Rol;
using user.common.Responses;
using user.dto.Role.v1;
using user.request.Querys.v1.Rol;

namespace user.handler.Rol.v1;
public class GetListRolesQueryHandler : IRequestHandler<GetListRolesQuery, ApiResponse<List<RoleListResponse>>>
{
    private readonly ILogger<GetListRolesQueryHandler> _logger;
    private readonly IRoleServie _roleServie;

    public GetListRolesQueryHandler(
        ILogger<GetListRolesQueryHandler> logger,
        IRoleServie roleServie)
    {
        _logger = logger;
        _roleServie = roleServie;
    }

    public async Task<ApiResponse<List<RoleListResponse>>> Handle(GetListRolesQuery request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Iniciando proceso de obtención de roles...");
        var roles = await _roleServie.ListRolesAsync(cancellationToken);
        if (roles == null)
        {
            _logger.LogWarning("Error en la obtención de roles");
            return ApiResponseHelper.CreateErrorResponse<List<RoleListResponse>>("Error al obtener los roles", 500);
        }
        var rolesResponse = roles.Select(x => new RoleListResponse
        {
            RoleId = x.RoleId,
            RoleName = x.RoleName,
            UserRoleValue = x.userRole
        }).ToList();
        return ApiResponseHelper.CreateSuccessResponse(rolesResponse, "Roles obtenidos correctamente");
    }
}
