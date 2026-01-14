using BuildingBlocks.CQRS;
using UserService.Infrastructure;

namespace UserService.Features.CheckPermissions;

public record CheckPermissionsQueryResponse(string Privelage);
public record CheckPermissionsQuery():IQuery<CheckPermissionsQueryResponse>;

public class CheckPermissionsQueryHandler(ICheckPermissionsService service,IDataRepository repository):IQueryHandler<CheckPermissionsQuery,CheckPermissionsQueryResponse>
{
    public async Task<CheckPermissionsQueryResponse> Handle(CheckPermissionsQuery query, CancellationToken cancellationToken)
    {
        var externalId = repository.ReadExternalIdFromToken();
        var result = await service.CheckPermissions(externalId);
        return new CheckPermissionsQueryResponse(result);
    }
}