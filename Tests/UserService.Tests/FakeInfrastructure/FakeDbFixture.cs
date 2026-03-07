using Microsoft.EntityFrameworkCore;
using Testcontainers.MsSql;
using UserService.Infrastructure;
using UserService.Tests.FakeBuilder;
using UserService.Tests.FakeRepository;

namespace UserService.Tests.FakeInfrastructure;

public sealed class FakeDbFixture:IAsyncLifetime
{
    private MsSqlContainer _container = default!;
    public string ConnectionString => _container.GetConnectionString();
    public ValueTask DisposeAsync()=>_container.DisposeAsync();
    public async  ValueTask InitializeAsync()
    {
        
        _container = new MsSqlBuilder("mcr.microsoft.com/mssql/server:2022-CU14-ubuntu-22.04")
            .Build();
        await _container.StartAsync();
        await using var db = CreateDbContext();
        await db.Database.MigrateAsync();
        var repo = new FakeDataRepository();
        var externalId = Guid.Parse("696e634a-a3a2-4b71-9471-bfcd3cfbe7be");
        var exists = await db.Users.AnyAsync(u => u.ExternalId == externalId);
        if (!exists)
        {
            var existing = new FakeUserBuilder().WithExternalId(externalId).Build();
            db.Users.Add(existing);
            await db.SaveChangesAsync();
        }
    }
    public UserDbContext CreateDbContext()
    {
        var options = new DbContextOptionsBuilder<UserDbContext>()
            .UseSqlServer(ConnectionString)
            .Options;

        return new UserDbContext(options);
    }
}