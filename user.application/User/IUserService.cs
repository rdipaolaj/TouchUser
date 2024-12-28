using user.request.Commands.v1;

namespace user.application.User;
public interface IUserService
{
    Task<user.entities.User?> CreateUserAsync(CreateUserCommand request, string salt, string hashedPassword, Guid roleId, CancellationToken cancellationToken);
    Task<entities.User?> GetUserByUsernameAsync(string username, CancellationToken cancellationToken);
}
