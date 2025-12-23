using System.Security.Claims;
using UserService.Commons.Dto;

namespace UserService.Infrastructure;

public sealed class CurrentUser(IHttpContextAccessor accessor):ICurrentUser
{
    private ClaimsPrincipal? User => accessor.HttpContext?.User;
    public bool IsAuthenticated => User?.Identity?.IsAuthenticated == true;
    public string Subject =>
        User?.FindFirstValue(ClaimTypes.NameIdentifier)
        ?? User?.FindFirstValue("sub")
        ?? throw new UnauthorizedAccessException("Missing subject claim (sub).");
    public string? Email =>
        User?.FindFirstValue(ClaimTypes.Email) ?? User?.FindFirstValue("email");
    public string? UserName =>
        User?.FindFirstValue("preferred_username")
        ?? User?.FindFirstValue(ClaimTypes.Name)
        ?? User?.FindFirstValue("name");
    }