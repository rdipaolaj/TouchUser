using Mapster;
using user.common.Responses;
using user.data.Repository.Interfaces;
using user.request.Commands.v1;

namespace user.application.User;
public class UserValidationService : IUserValidationService
{
    private readonly IUserValidationRepository _userValidationRepository;

    public UserValidationService(IUserValidationRepository userValidationRepository)
    {
        _userValidationRepository = userValidationRepository;
    }

    public async Task<ApiResponse<string>> ValidateUserAsync(CreateUserCommand request, CancellationToken cancellationToken)
    {
        var user = request.Adapt<user.entities.User>();
        user.Username = request.Username;
        user.Email = request.Email;
        user.PhoneNumber = request.PhoneNumber;

        var response = await _userValidationRepository.ValidateUserAsync(user, cancellationToken);
        return response;
    }
}
