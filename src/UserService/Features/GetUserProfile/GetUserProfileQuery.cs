using BuildingBlocks.CQRS;
using BuildingBlocks.Security.ClientToService.CurrentUser;
using UserService.Commons.Dto;
using UserService.Infrastructure;

namespace UserService.Features.GetUserProfile;

public sealed record GetUserProfileQueryResponse(ProfileDto Dto);
public record GetUserProfileQuery():IQuery<GetUserProfileQueryResponse>;

public class GetUserProfileQueryHandler(IGetUserProfileService service,IDataRepository repository) : IQueryHandler<GetUserProfileQuery, GetUserProfileQueryResponse>
{
    public async  Task<GetUserProfileQueryResponse> Handle(GetUserProfileQuery query, CancellationToken cancellationToken)
    {
        var externalId = repository.ReadExternalIdFromToken();
        var userDto =await  service.GetUserProfile(externalId, cancellationToken);
        return new GetUserProfileQueryResponse(userDto);
    }
}
