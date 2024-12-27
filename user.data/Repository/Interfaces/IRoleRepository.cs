using user.common.Enums;
using user.entities;

namespace user.data.Repository.Interfaces;
public interface IRoleRepository
{
    Task<Role?> GetRoleIdByGuidAsync(UserRole rolid, CancellationToken cancellationToken);
}
