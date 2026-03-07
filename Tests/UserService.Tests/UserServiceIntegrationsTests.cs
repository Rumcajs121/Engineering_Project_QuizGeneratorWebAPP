using Microsoft.EntityFrameworkCore;
using UserService.Tests.FakeInfrastructure;

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
}