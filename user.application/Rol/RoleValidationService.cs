using user.common.Enums;
using user.common.Responses;
using user.data.Repository.Interfaces;
using user.entities;

namespace user.application.Rol;
public class RoleValidationService : IRoleValidationService
{
    private readonly IRoleRepository _roleService;

    public RoleValidationService(IRoleRepository roleService)
    {
        _roleService = roleService;
    }

    public async Task<ApiResponse<Role>> ValidateRoleAsync(int rolid, CancellationToken cancellationToken)
    {
        var roleActions = new Dictionary<int, UserRole>
        {
            { (int)UserRole.Employee, UserRole.Employee },
            { (int)UserRole.Administrator, UserRole.Administrator }
        };

        if (!roleActions.TryGetValue(rolid, out UserRole userRole))
        {
            var validationErrors = new List<ErrorDetail>
            {
                new ErrorDetail { Code = "USER0013", Description = "El Rol proporcionado no es válido." }
            };
            return ApiResponseHelper.CreateValidationErrorResponse<Role>(validationErrors);
        }

        var role = await _roleService.GetRoleIdByGuidAsync(userRole, cancellationToken);

        if (role is null)
        {
            var validationErrors = new List<ErrorDetail>
            {
                new ErrorDetail { Code = "USER0012", Description = "El Rol no existe." }
            };
            return ApiResponseHelper.CreateValidationErrorResponse<Role>(validationErrors);
        }
        return ApiResponseHelper.CreateSuccessResponse(role, "Rol validado.");
    }
}
