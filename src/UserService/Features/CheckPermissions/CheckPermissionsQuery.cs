using BuildingBlocks.CQRS;

namespace UserService.Features.CheckPermissions;

public record CheckPermissionsQueryRequest(string ExternalId);
public record CheckPermissionsQueryResponse(string Privilege);
public record CheckPermissionsQuery(CheckPermissionsQueryRequest ExternalId):IQuery<CheckPermissionsQueryResponse>;

public class CheckPermissionsQueryHandler:IQueryHandler<CheckPermissionsQuery,CheckPermissionsQueryResponse>
{
    public Task<CheckPermissionsQueryResponse> Handle(CheckPermissionsQuery query, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}