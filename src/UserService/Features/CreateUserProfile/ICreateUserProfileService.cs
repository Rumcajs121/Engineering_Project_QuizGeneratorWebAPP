using UserService.Commons.Models;
using UserService.Infrastructure;

namespace UserService.Features.CreateUserProfile;

public interface ICreateUserProfileService
{
    Task CreateUser(CancellationToken ct);
}

public class CreateUserProfileService(IDataRepository repository) : ICreateUserProfileService
{
    public async Task CreateUser(CancellationToken ct)
    {
        var userContext = repository.ReadUserFromTheToken();
        var existing = await repository.GetUserForUpdateAsync(userContext.Subject,ct);

        if (existing is null)
        {
            var user = new UserDomain
            {
                ExternalId = userContext.Subject,
                Username = userContext.UserName,
                Email = userContext.Email,
                PrivilegeUserDomain = PrivilegesUserDomain.User,
                IsActive = true,
                LastLoginAt = DateTime.UtcNow
            };

            await repository.AddUserForDbAsync(user);
        }
        else
        {
            existing.LastLoginAt = DateTime.UtcNow;
        }
        await repository.DbSaveAsync();
    }
}