using BuildingBlocks.Security.ClientToService.CurrentUser;

namespace UserService.Tests.FakeRepository;

public class FakeCurrentUser:ICurrentUser
{
    public string Subject { get; init; } = Guid.NewGuid().ToString();
    public string? Email { get; init; } = "test@local";
    public string? UserName { get; init; } = "test-user";
    public bool IsAuthenticated { get; init; } = true;
}