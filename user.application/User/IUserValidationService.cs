using user.common.Responses;
using user.request.Commands.v1;

namespace user.application.User;
public interface IUserValidationService
{
    Task<ApiResponse<string>> ValidateUserAsync(CreateUserCommand request, CancellationToken cancellationToken);
}
