using Mapster;
using user.common.Constant;
using user.data.Repository.Interfaces;
using user.request.Commands.v1;

namespace user.application.User;
internal class UserService : IUserService
{
    private readonly IUserRepository _userRepository;

    public UserService(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<entities.User?> CreateUserAsync(CreateUserCommand request, string salt, string hashedPassword, Guid roleId, CancellationToken cancellationToken)
    {
        var user = request.Adapt<entities.User>();
        user.Salt = salt;
        user.HashedPassword = hashedPassword;
        user.RoleId = roleId;
        user.AccountStatus = CommonConstant.Active;

        var response = await _userRepository.CreateUserAsync(user, cancellationToken);
        return response;
    }

    /// <summary>
    /// Obtiene un usuario en base al username.
    /// </summary>
    public async Task<entities.User?> GetUserByUsernameAsync(string username, CancellationToken cancellationToken)
    {
        return await _userRepository.GetUserByUsernameAsync(username, cancellationToken);
    }

    public async Task<IEnumerable<string>> GetAdminEmailsAsync(CancellationToken cancellationToken)
    {
        return await _userRepository.GetAdminEmailsAsync(cancellationToken);
    }
}
