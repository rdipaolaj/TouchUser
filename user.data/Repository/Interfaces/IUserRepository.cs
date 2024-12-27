using user.entities;

namespace user.data.Repository.Interfaces;
public interface IUserRepository
{
    Task<User?> CreateUserAsync(User user, CancellationToken cancellationToken);
}
