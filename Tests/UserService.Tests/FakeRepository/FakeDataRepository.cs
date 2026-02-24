using UserService.Commons.Dto;
using UserService.Commons.Models;
using UserService.Infrastructure;

namespace UserService.Tests.FakeRepository;

public class FakeDataRepository:IDataRepository
{
    private readonly Dictionary<Guid, UserDomain> Users = [];
    public int AddUserCalls { get; private set; }
    public int SaveCalls { get; private set; }
    public List<UserDomain> AddedUsers { get; } = new();
    public FakeDataRepository()
    {
        var fixedNow = new DateTime(2026, 02, 24, 12, 0, 0, DateTimeKind.Utc);
        var userDomain = new UserDomain
        {
            UserId = Guid.Parse("8bb63582-8d1d-4ed7-9639-9f5a41a2d3d4"),
            ExternalId = Guid.Parse("35aa1e30-3af8-4486-a236-9b8736570e3e"),
            Username = "Testowy",
            Email = "testowy@test.com",
            PrivilegeUserDomain = PrivilegesUserDomain.User,
            IsActive = true,
            CreatedAt = fixedNow.AddDays(-14),
            LastLoginAt = fixedNow.AddDays(-7)
        };
        Users[userDomain.ExternalId] = userDomain;
    }
    public Task<UserDomain?> GetUserReadAsync(Guid externalId, CancellationToken ct)
        => Task.FromResult(Users.TryGetValue(externalId, out var user) ? user : null);

    public Task<UserDomain?> GetUserForUpdateAsync(Guid externalId, CancellationToken ct)
        => Task.FromResult(Users.TryGetValue(externalId, out var user) ? user : null);

    public Task AddUserForDbAsync(UserDomain user)
    {
        AddUserCalls++;
        AddedUsers.Add(user);
        Users[user.ExternalId] = user;
        return Task.CompletedTask;
    }

    public Task<PrivilegesUserDomain> GetPrivileges(Guid externalId)
    {
        if (!Users.TryGetValue(externalId, out var user))
            throw new InvalidOperationException("Sequence contains no elements"); 

        return Task.FromResult(user.PrivilegeUserDomain);
    }

    public Task DbSaveAsync()
    {
        SaveCalls++;
        return Task.CompletedTask;
    }

    public UserContextDto ReadUserFromTheToken()
    {
        var isAuthenticated = true;
        var subject = Guid.Parse("696e634a-a3a2-4b71-9471-bfcd3cfbe7be");
        var email = "testowy1@test.com";
        var userName = "testowy1";
        var result = new UserContextDto(email, subject,userName , isAuthenticated);
        return result;
    }

    public Guid ReadExternalIdFromToken()
    {
        var subject = Guid.Parse("696e634a-a3a2-4b71-9471-bfcd3cfbe7be");
        return subject;
    }
}