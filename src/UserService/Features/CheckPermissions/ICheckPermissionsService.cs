using UserService.Commons.Models;
using UserService.Infrastructure;

namespace UserService.Features.CheckPermissions;

public interface ICheckPermissionsService
{
    Task<bool> CheckPermissions(string externalId, string permission);
}

public class CheckPermissionsService(IDataRepository repository):ICheckPermissionsService
{
    public async Task<bool> CheckPermissions(string externalId, string permission)
    {
        if (!Enum.TryParse<PrivilegesUserDomain>(permission, ignoreCase:true, out var required))
        {
            return false;
        }
        var dbCheckPermissions = await repository.GetPrivileges(externalId);
        return dbCheckPermissions >= required;
    }
}