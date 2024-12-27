using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using user.common.Enums;

namespace user.entities;
public class User
{
    [Key]
    public Guid UserId { get; set; }
    public string Username { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string PhoneNumber { get; set; } = string.Empty;
    public string HashedPassword { get; set; } = string.Empty;
    public string Salt { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }
    public string AccountStatus { get; set; } = string.Empty;
    public Guid RoleId { get; set; }
    public Role Role { get; set; } = default!;
}

public class Role
{
    [Key]
    public Guid RoleId { get; set; }
    public string RoleName { get; set; } = string.Empty;
    public UserRole userRole { get; set; }

    [JsonIgnore]
    public List<User> Users { get; set; } = new List<User>();
}