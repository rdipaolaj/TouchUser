using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using user.common.Enums;
using user.data.Repository.Interfaces;
using user.entities;

namespace user.data.Repository;
public class RoleRepository : IRoleRepository
{
    private readonly ApplicationDbContext _context;
    private readonly ILogger<RoleRepository> _logger;

    public RoleRepository(ApplicationDbContext context, ILogger<RoleRepository> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<Role?> GetRoleIdByGuidAsync(UserRole rolid, CancellationToken cancellationToken)
    {
        try
        {
            _logger.LogInformation("GetRoleIdByGuidAsync iniciado");
            Role? role = await _context.Roles.FirstOrDefaultAsync(x => x.userRole == rolid, cancellationToken);
            return role;
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error en GetRoleIdByGuidAsync: {ex.Message}");
            return null;
        }
        
    }
}
