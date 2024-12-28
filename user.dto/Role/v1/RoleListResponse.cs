using user.common.Enums;

namespace user.dto.Role.v1;
public class RoleListResponse
{
    public Guid RoleId { get; set; }
    public string RoleName { get; set; } = string.Empty;
    public UserRole UserRoleValue { get; set; }
}
