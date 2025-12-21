namespace UserService.Commons.Models;

public class UserDomain
{
    public Guid UserId => Guid.NewGuid();
    public string ExternalId  { get; set; } // With JWT token claim  "preferred_username  "sub"
    public string Username { get; set; } // With JWT token claim  "preferred_username"
    public string Email { get; set; } // With JWT token claim  "email"
    public PrivilegesUserDomain PrivilegeUserDomain { get; set; } 
    public bool IsActive { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? LastLoginAt { get; set; }
}