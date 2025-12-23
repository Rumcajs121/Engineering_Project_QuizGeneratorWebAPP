using UserService.Commons.Models;
using UserService.Infrastructure;

namespace UserService.Features.CreateUserProfile;

public interface ICreateUserProfileService
{
    Task CreateUser( string subject, string email, string userName,CancellationToken ct);
}

public class CreateUserProfileService(IDataRepository repository) : ICreateUserProfileService
{
    public async Task CreateUser(string subject, string email, string userName, CancellationToken ct)
    {
        var existing = await repository.GetUserForUpdateAsync(subject);

        if (existing is null)
        {
            var user = new UserDomain
            {
                ExternalId = subject,
                Username = userName,
                Email = email,
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