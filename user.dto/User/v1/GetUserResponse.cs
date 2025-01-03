﻿namespace user.dto.User.v1;

public class GetUserResponse
{
    public Guid UserId { get; set; }
    public string Username { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string PhoneNumber { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public string AccountStatus { get; set; } = string.Empty;
    public Guid RoleId { get; set; }
}
