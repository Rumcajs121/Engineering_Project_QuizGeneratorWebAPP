using UserService.Commons.Dto;
using UserService.Infrastructure;

namespace UserService.Features.EditProfile;

public interface IEditProfileService
{
    Task EditProfile(EditProfileDto dto,CancellationToken ct);
}

public class EditProfileService(IDataRepository repository) : IEditProfileService
{
    public async Task EditProfile(EditProfileDto dto,CancellationToken ct)
    {
        var externalId = repository.ReadExternalIdFromToken();
        var user = await repository.GetUserForUpdateAsync(externalId,ct);
        if (dto.PrivilegeUserDomain is not null)
            user.PrivilegeUserDomain = dto.PrivilegeUserDomain.Value;
        if (dto.IsActive is not null)
            user.IsActive = dto.IsActive.Value;
        if (dto.Email is not null)
            user.Email = dto.Email;
        if (dto.Username is not null)
            user.Username = dto.Username;
        await repository.DbSaveAsync();
    }
}