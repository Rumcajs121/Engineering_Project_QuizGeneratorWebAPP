using UserService.Commons.Models;

namespace UserService.Commons.Dto;

public  class EditProfileDto
{
    public PrivilegesUserDomain PrivilegeUserDomain { get; set; } 
    public bool IsActive { get; set; }
    public required string Username { get; set; }
    public required string Email { get; set; }
}