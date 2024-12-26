using MediatR;
using user.common.Responses;
using user.dto.User.v1;

namespace user.request.Commands.v1;
public class CreateUserCommand : IRequest<ApiResponse<CreateUserResponse>>
{
    public string Username { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string PhoneNumber { get; set; } = string.Empty;
    public string HashedPassword { get; set; } = string.Empty;
    public string UserRole { get; set; } = string.Empty;
}