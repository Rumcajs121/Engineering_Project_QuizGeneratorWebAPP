using BuildingBlocks.Security.ClientToService.CurrentUser;
using Microsoft.EntityFrameworkCore;
using UserService.Commons.Dto;
using UserService.Commons.Models;

namespace UserService.Infrastructure;

public interface IDataRepository
{
    Task<UserDomain?>  GetUserReadAsync(Guid externalId,CancellationToken ct);
    Task<UserDomain?> GetUserForUpdateAsync(Guid externalId,CancellationToken ct);
    Task AddUserForDbAsync(UserDomain user);
    Task<PrivilegesUserDomain> GetPrivileges(Guid externalId);
    Task  DbSaveAsync();
    UserContextDto ReadUserFromTheToken();
    Guid ReadExternalIdFromToken();
}

public class DataRepository(UserDbContext dbContext, ICurrentUser currentUser) : IDataRepository
{
    public async Task<UserDomain?> GetUserReadAsync(Guid externalId, CancellationToken ct) =>
        await dbContext.Users.AsNoTracking().FirstOrDefaultAsync(x => x.ExternalId == externalId, ct);

    public Task<UserDomain?> GetUserForUpdateAsync(Guid externalId, CancellationToken ct) =>
        dbContext.Users.FirstOrDefaultAsync(u => u.ExternalId == externalId, ct);

    public async Task AddUserForDbAsync(UserDomain user)
    {
        await dbContext.Users.AddAsync(user);
    }

    public async Task<PrivilegesUserDomain> GetPrivileges(Guid externalId)
    {
        PrivilegesUserDomain userPrivileges = await dbContext.Users.AsNoTracking()
            .Where(x => x.ExternalId == externalId).Select(x => x.PrivilegeUserDomain).SingleAsync();
        return userPrivileges;
    }

    public async Task DbSaveAsync()
    {
        await dbContext.SaveChangesAsync();
    }


    public UserContextDto ReadUserFromTheToken()
    {
        var isAuthenticated = currentUser.IsAuthenticated;
        var subject = Guid.Parse(currentUser.Subject);
        var email = currentUser.Email;
        var userName = currentUser.UserName;
        var result = new UserContextDto(email, subject,userName , isAuthenticated);
        return result;
    }

    public Guid ReadExternalIdFromToken()
    {
        var externalId = Guid.Parse(currentUser.Subject);
        return externalId;
    }
}