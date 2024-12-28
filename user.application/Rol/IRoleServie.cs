using user.entities;

namespace user.application.Rol;
public interface IRoleServie
{
    Task<List<Role?>?> ListRolesAsync(CancellationToken cancellationToken);
}
