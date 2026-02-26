using UserService.Commons.Models;

namespace UserService.Tests.FakeBuilder;

public class FakeUserBuilder
{
    private Guid _userId = Guid.NewGuid();
    private Guid _externalId = Guid.NewGuid();
    private string _username = "test-user";
    private string _email = "test@local";
    private PrivilegesUserDomain _privilege = PrivilegesUserDomain.User;
    private bool _isActive = true;
    private DateTime _createdAt = DateTime.UtcNow.AddDays(-8);
    private DateTime? _lastLoginAt = DateTime.UtcNow.AddDays(-1);
    public FakeUserBuilder WithExternalId(Guid externalId) { _externalId = externalId; return this; }
    public FakeUserBuilder WithUsername(string username) { _username = username; return this; }
    public FakeUserBuilder WithEmail(string email) { _email = email; return this; }
    public FakeUserBuilder Inactive() { _isActive = false; return this; }
    public FakeUserBuilder WithPrivilege(PrivilegesUserDomain privilege) { _privilege = privilege; return this; }

    public UserDomain Build() => new UserDomain
    {
        UserId = _userId,
        ExternalId = _externalId,
        Username = _username,
        Email = _email,
        PrivilegeUserDomain = _privilege,
        IsActive = _isActive,
        CreatedAt = _createdAt,
        LastLoginAt = _lastLoginAt
    };
}