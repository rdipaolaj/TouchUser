using user.entities;

namespace user.redis.Users;
public interface IRedisUserService
{
    Task SyncUserWithRedisAsync(User user, string username);
}
