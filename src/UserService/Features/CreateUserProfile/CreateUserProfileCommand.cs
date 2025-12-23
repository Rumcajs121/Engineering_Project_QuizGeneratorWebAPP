using BuildingBlocks.CQRS;
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
        if (!currentUser.IsAuthenticated)
            throw new UnauthorizedAccessException();
        var subject = currentUser.Subject;
        var email = currentUser.Email;
        var userName = currentUser.UserName;

        await service.CreateUser(subject, email, userName, cancellationToken);

        return new CreateUserProfileCommandResponse(true);
    }
}