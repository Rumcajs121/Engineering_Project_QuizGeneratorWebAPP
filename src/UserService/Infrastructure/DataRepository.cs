using Microsoft.EntityFrameworkCore;
using UserService.Commons.Models;

namespace UserService.Infrastructure;

public interface IDataRepository
{
    Task<UserDomain?>  GetUserReadAsync(string externalId,CancellationToken ct);
    Task<UserDomain?> GetUserForUpdateAsync(string externalId,CancellationToken ct);
    Task AddUserForDbAsync(UserDomain user);
    Task<PrivilegesUserDomain> GetPrivileges(string externalIdId);
    Task  DbSaveAsync();
}
public class DataRepository(UserDbContext dbContext):IDataRepository
{
    public async Task<UserDomain?> GetUserReadAsync(string externalId,CancellationToken ct)=>await dbContext.Users.AsNoTracking().FirstOrDefaultAsync(x => x.ExternalId == externalId,ct);
    public Task<UserDomain?> GetUserForUpdateAsync(string externalId,CancellationToken ct) => dbContext.Users.FirstOrDefaultAsync(u => u.ExternalId == externalId,ct);
    public async Task AddUserForDbAsync(UserDomain user)
    {
        await dbContext.Users.AddAsync(user);
    }
    public async Task<PrivilegesUserDomain> GetPrivileges(string externalId)
    {
        PrivilegesUserDomain userPrivileges=await dbContext.Users.AsNoTracking().Where(x=>x.ExternalId==externalId).Select(x=>x.PrivilegeUserDomain).SingleAsync();
        return userPrivileges;
    }
    public async Task DbSaveAsync()
    {
        await dbContext.SaveChangesAsync();
    }
}