using Microsoft.Extensions.Logging;
using user.data.Repository.Interfaces;
using user.entities;

namespace user.data.Repository;
public class UserRepository : IUserRepository
{
	private readonly ApplicationDbContext _context;
    private readonly ILogger<UserRepository> _logger;

    public UserRepository(ApplicationDbContext context, ILogger<UserRepository> logger)
    {
        _context = context;
        _logger = logger;
    }
    public async Task<User?> CreateUserAsync(User user, CancellationToken cancellationToken)
    {
		try
		{
            _logger.LogInformation("CreateUserAsync iniciado");
            await _context.Users.AddAsync(user, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
            return user;
        }
		catch (Exception ex)
		{
            _logger.LogError($"Error en CreateUserAsync: {ex.Message}");
            return null;
        }
    }
}
