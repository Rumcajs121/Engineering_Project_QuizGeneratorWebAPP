using UserService.Commons.Models;
using UserService.Infrastructure;

namespace UserService.Features.CheckPermissions;

public interface ICheckPermissionsService
{
    Task<string> CheckPermissions(Guid externalId);
}

public class CheckPermissionsService(IDataRepository repository):ICheckPermissionsService
{
    public async Task<string> CheckPermissions(Guid externalId)
    {
        var dbCheckPermissions = await repository.GetPrivileges(externalId);
        return dbCheckPermissions.ToString();
    }
}