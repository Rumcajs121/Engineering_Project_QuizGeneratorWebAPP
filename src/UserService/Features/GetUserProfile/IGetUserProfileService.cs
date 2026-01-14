using BuildingBlocks.Exceptions;
using UserService.Commons.Dto;
using UserService.Infrastructure;

namespace UserService.Features.GetUserProfile;

public interface IGetUserProfileService
{
    Task<ProfileDto>GetUserProfile(Guid externalId,CancellationToken ct);
}

public class GetUserProfileService(IDataRepository repository) : IGetUserProfileService
{
    public async Task<ProfileDto> GetUserProfile(Guid externalId,CancellationToken ct)
    {
        var userDomain= await repository.GetUserReadAsync(externalId,ct);
        if (userDomain != null)
        {
            var userDto=new ProfileDto(userDomain.UserId,userDomain.Username,userDomain.Email,userDomain.PrivilegeUserDomain,userDomain.IsActive,userDomain.LastLoginAt);
            return userDto;
        }
        throw new NotFoundException($"Requested resource was not found for {externalId}.");
    }
}