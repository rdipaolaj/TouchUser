using MediatR;
using user.common.Responses;

namespace user.request.Querys.v1.User;
public class ListAdminEmailsQuery : IRequest<ApiResponse<IEnumerable<string>>>
{
}
