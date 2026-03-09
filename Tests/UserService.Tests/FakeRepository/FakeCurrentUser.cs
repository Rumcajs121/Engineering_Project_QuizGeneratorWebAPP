using BuildingBlocks.Security.ClientToService.CurrentUser;

namespace UserService.Tests.FakeRepository;

public class FakeCurrentUser:ICurrentUser
{
    public string Subject { get; init; } = "696e634a-a3a2-4b71-9471-bfcd3cfbe7be";
    public string? Email { get; init; } = "test@local";
    public string? UserName { get; init; } = "test-user";
    public bool IsAuthenticated { get; init; } = true;
}