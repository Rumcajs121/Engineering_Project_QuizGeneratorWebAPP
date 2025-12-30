using BuildingBlocks.CQRS;
using UserService.Commons.Dto;

namespace UserService.Features.GetUserProfile;

public sealed record GetUserProfileQueryResponse(ProfileDto Dto);
public record GetUserProfileQuery():IQuery<GetUserProfileQueryResponse>;

public class GetUserProfileQueryHandler(IGetUserProfileService service,ICurrentUser currentUser) : IQueryHandler<GetUserProfileQuery, GetUserProfileQueryResponse>
{
    public async  Task<GetUserProfileQueryResponse> Handle(GetUserProfileQuery query, CancellationToken cancellationToken)
    {
        if (!currentUser.IsAuthenticated)
            throw new UnauthorizedAccessException();
        var userDto =await  service.GetUserProfile(currentUser.Subject, cancellationToken);
        return new GetUserProfileQueryResponse(userDto);
    }
}
