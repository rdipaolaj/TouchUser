using MediatR;
using user.common.Responses;
using user.dto.Role.v1;

namespace user.request.Querys.v1.Rol;
public class GetListRolesQuery : IRequest<ApiResponse<List<RoleListResponse>>>
{
}
