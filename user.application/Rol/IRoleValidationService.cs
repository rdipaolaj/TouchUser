using user.common.Responses;
using user.entities;

namespace user.application.Rol;
public interface IRoleValidationService
{
    Task<ApiResponse<Role>> ValidateRoleAsync(int rolid, CancellationToken cancellationToken);
}
