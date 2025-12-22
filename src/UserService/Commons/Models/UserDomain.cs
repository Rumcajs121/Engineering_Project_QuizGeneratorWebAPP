namespace UserService.Commons.Models;

public class UserDomain
{
    
    public Guid UserId { get; set; } = Guid.NewGuid();
    public required string ExternalId  { get; set; } // With JWT token claim  "preferred_username  "sub"
    public required string Username { get; set; } // With JWT token claim  "preferred_username"
    public required string Email { get; set; } // With JWT token claim  "email"
    public PrivilegesUserDomain PrivilegeUserDomain { get; set; } 
    public bool IsActive { get; set; }
    public DateTime CreatedAt { get; set; }=DateTime.UtcNow;
    public DateTime? LastLoginAt { get; set; }
}