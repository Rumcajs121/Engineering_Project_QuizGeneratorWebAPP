using UserService.Commons.Models;

namespace UserService.Commons.Dto;

public  class EditProfileDto
{
    public PrivilegesUserDomain PrivilegeUserDomain { get; init; } 
    public bool IsActive { get; init; }
    public required string Username { get; init; }
    public required string Email { get; init; }
}