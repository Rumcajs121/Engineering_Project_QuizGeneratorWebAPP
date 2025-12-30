namespace UserService.Commons.Models;

public class UserDomain
{

    public Guid UserId { get; set; } = Guid.NewGuid();
    public required string ExternalId  { get; set; } 
    public required string Username { get; set; } 
    public required string Email { get; set; } 
    public PrivilegesUserDomain PrivilegeUserDomain { get; set; } 
    public bool IsActive { get; set; }
    public DateTime CreatedAt { get; set; }=DateTime.UtcNow;
    public DateTime? LastLoginAt { get; set; }
}