using UserService.Commons.Dto;
using UserService.Infrastructure;

namespace UserService.Features.EditProfile;

public interface IEditProfileService
{
    Task EditProfile(string externalId,EditProfileDto dto,CancellationToken ct);
}

public class EditProfileService(IDataRepository repository) : IEditProfileService
{
    public async Task EditProfile(string externalId,EditProfileDto dto,CancellationToken ct)
    {
        var user = await repository.GetUserForUpdateAsync(externalId,ct);
        if (user != null)
        {
            user.PrivilegeUserDomain = dto.PrivilegeUserDomain;
            user.IsActive = dto.IsActive;
            user.Email = dto.Email;
            user.Username = dto.Username;
        }

        await repository.DbSaveAsync();
    }
}