using UserService.Commons.Dto;
using UserService.Commons.Models;
using UserService.Infrastructure;

namespace UserService.Tests.FakeRepository;

public class FakeDataRepository:IDataRepository
{
    private readonly Dictionary<Guid, UserDomain> _users = [];
    public int AddUserCalls { get; private set; }
    public int SaveCalls { get; private set; }
    public List<UserDomain> AddedUsers { get; } = [];
    public Guid TokenSubject { get; set; } = Guid.NewGuid();
    public string TokenEmail { get; set; } = "test@local";
    public string TokenUserName { get; set; } = "test-user";
    public bool TokenIsAuthenticated { get; set; } = true;
    
    public void Seed(UserDomain user) => _users[user.ExternalId] = user;
    public Task<UserDomain?> GetUserReadAsync(Guid externalId, CancellationToken ct)
        => Task.FromResult(_users.TryGetValue(externalId, out var user) ? user : null);

    public Task<UserDomain?> GetUserForUpdateAsync(Guid externalId, CancellationToken ct)
        => Task.FromResult(_users.TryGetValue(externalId, out var user) ? user : null);

    public Task AddUserForDbAsync(UserDomain user)
    {
        AddUserCalls++;
        AddedUsers.Add(user);
        _users[user.ExternalId] = user;
        return Task.CompletedTask;
    }

    public Task<PrivilegesUserDomain> GetPrivileges(Guid externalId)
    {
        if (!_users.TryGetValue(externalId, out var user))
            throw new InvalidOperationException("Sequence contains no elements"); 

        return Task.FromResult(user.PrivilegeUserDomain);
    }

    public Task DbSaveAsync()
    {
        SaveCalls++;
        return Task.CompletedTask;
    }

    public UserContextDto ReadUserFromTheToken() =>
        new(TokenEmail, TokenSubject, TokenUserName, TokenIsAuthenticated);

    public Guid ReadExternalIdFromToken()
    {
        var subject = Guid.Parse("696e634a-a3a2-4b71-9471-bfcd3cfbe7be");
        return subject;
    }
    public UserDomain? GetUser(Guid externalId)
        => _users.TryGetValue(externalId, out var user) ? user : null;

    public bool Contains(Guid externalId)
        => _users.ContainsKey(externalId);
}