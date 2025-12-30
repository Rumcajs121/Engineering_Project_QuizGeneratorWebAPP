using UserService.Commons.Models;

namespace UserService.Commons.Dto;

public sealed record ProfileDto(
    Guid UserId,
    string Username,
    string Email,
    PrivilegesUserDomain PrivilegeUserDomain,
    bool IsActive,
    DateTime? LastLoginAt
);