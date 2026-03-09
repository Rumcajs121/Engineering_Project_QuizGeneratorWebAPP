using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using UserService.Features.GetUserProfile;
using UserService.Infrastructure;
using UserService.Tests.FakeInfrastructure;
using UserService.Tests.FakeRepository;

namespace UserService.Tests;

[Collection("db")]
public class UserServiceIntegrationsTests
{
    private readonly FakeDbFixture _fx;

    public UserServiceIntegrationsTests(FakeDbFixture db)
    {
        _fx = db;
    }

    [Fact]
    public async Task Check_Connected_Db_And_Seed()
    {
        var externalId = Guid.Parse("696e634a-a3a2-4b71-9471-bfcd3cfbe7be");

        await using var db = _fx.CreateDbContext();

        var user = await db.Users.SingleOrDefaultAsync(x => x.ExternalId == externalId);

        Assert.NotNull(user);
    }

    [Fact]
    public async Task Check_Get_Privileges_Value_in_DataRepository()
    {
        await using var db = _fx.CreateDbContext();
        var currentUser = new FakeCurrentUser();
        var externalId = Guid.Parse(currentUser.Subject);
        var expected = await db.Users.AsNoTracking()
            .Where(x => x.ExternalId == externalId)
            .Select(x => x.PrivilegeUserDomain)
            .SingleAsync(cancellationToken: TestContext.Current.CancellationToken);
        var repository = new DataRepository(db, currentUser);
        var result = await repository.GetPrivileges(externalId);

        result.Should().Be(expected);
    }

    [Fact]
    public async Task Handle_returns_user_profile_from_database()
    {
        var ct = TestContext.Current.CancellationToken;
        await using var db = _fx.CreateDbContext();
        var currentUser = new FakeCurrentUser();
        var externalId = Guid.Parse(currentUser.Subject);
        IDataRepository repository = new DataRepository(db, currentUser);
        IGetUserProfileService service=new GetUserProfileService(repository);
        var handler=new GetUserProfileQueryHandler(service,repository);
        var expected = await db.Users.AsNoTracking()
            .SingleAsync(x => x.ExternalId == externalId,ct);
        
        var response = await handler.Handle(new GetUserProfileQuery(), ct);
        
        response.Dto.UserId.Should().Be(expected.UserId);
        response.Dto.PrivilegeUserDomain.Should().Be(expected.PrivilegeUserDomain);
        response.Dto.Email.Should().Be(expected.Email);
    }
}