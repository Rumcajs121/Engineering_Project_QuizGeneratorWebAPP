using BuildingBlocks.CQRS;

namespace UserService.Features.CheckPermissions;

public record CheckPermissionsQueryRequest(string ExternalId,string Privilege);
public record CheckPermissionsQueryResponse(bool Success);
public record CheckPermissionsQuery(CheckPermissionsQueryRequest Permissions):IQuery<CheckPermissionsQueryResponse>;

public class CheckPermissionsQueryHandler(ICheckPermissionsService service):IQueryHandler<CheckPermissionsQuery,CheckPermissionsQueryResponse>
{
    public async Task<CheckPermissionsQueryResponse> Handle(CheckPermissionsQuery query, CancellationToken cancellationToken)
    {
        var result = await service.CheckPermissions(query.Permissions.ExternalId,query.Permissions.Privilege);
        return new CheckPermissionsQueryResponse(result);
    }
}