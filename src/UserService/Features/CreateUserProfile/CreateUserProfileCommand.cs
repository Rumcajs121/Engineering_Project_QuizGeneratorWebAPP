using BuildingBlocks.CQRS;
using BuildingBlocks.Security.ClientToService.CurrentUser;
using UserService.Commons.Dto;

namespace UserService.Features.CreateUserProfile;


public record CreateUserProfileCommandResponse(bool Success);
public record CreateUserProfileCommand: ICommand<CreateUserProfileCommandResponse>;

public class
    CreateUserProfileCommandHandler(ICreateUserProfileService service,ICurrentUser currentUser)
    : ICommandHandler<CreateUserProfileCommand, CreateUserProfileCommandResponse>
{
    public async Task<CreateUserProfileCommandResponse> Handle(CreateUserProfileCommand request, CancellationToken cancellationToken)
    {
        await service.CreateUser(cancellationToken);
        return new CreateUserProfileCommandResponse(true);
    }
}