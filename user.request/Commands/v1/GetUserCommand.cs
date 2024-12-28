using MediatR;
using user.common.Responses;
using user.dto.User.v1;

namespace user.request.Commands.v1;
public class GetUserCommand : IRequest<ApiResponse<GetUserResponse>>
{
    public string Username { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
}
