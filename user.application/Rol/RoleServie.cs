using Microsoft.Extensions.Logging;
using user.data.Repository.Interfaces;
using user.entities;

namespace user.application.Rol;
public class RoleServie : IRoleServie
{
    private readonly ILogger<RoleServie> _logger;
    private readonly IRoleRepository _roleRepository;

    public RoleServie(
        ILogger<RoleServie> logger,
        IRoleRepository roleRepository)
    {
        _logger = logger;
        _roleRepository = roleRepository;
    }

    public async Task<List<Role?>?> ListRolesAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("ListRolesAsync iniciado");
        List<Role?>? roles = await _roleRepository.GetListRoleAsync(cancellationToken);
        if (roles == null) {
            _logger.LogError("Error en ListRolesAsync");
            return null;
        }
        return roles;
    }
}
