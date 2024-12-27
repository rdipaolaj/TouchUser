using user.common.Responses;
using user.entities;

namespace user.data.Repository.Interfaces;
public interface IUserValidationRepository
{
    Task<ApiResponse<string>> ValidateUserAsync(User user, CancellationToken cancellationToken);
}
