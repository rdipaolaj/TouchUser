using user.entities;

namespace user.data.Repository.Interfaces;
public interface IUserRepository
{
    Task<User?> CreateUserAsync(User user, CancellationToken cancellationToken);
    Task<User?> GetUserByUsernameAsync(string username, CancellationToken cancellationToken);
    Task<IEnumerable<string>> GetAdminEmailsAsync(CancellationToken cancellationToken);
}
