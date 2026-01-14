namespace BuildingBlocks.Security.ClientToService.CurrentUser;

public interface ICurrentUser
{
    string Subject { get; } 
    string? Email { get; }
    string? UserName { get; }
    bool IsAuthenticated { get; }
}