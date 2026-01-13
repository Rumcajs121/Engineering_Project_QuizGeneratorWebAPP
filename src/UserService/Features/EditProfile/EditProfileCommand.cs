using BuildingBlocks.CQRS;
using UserService.Commons.Dto;


namespace UserService.Features.EditProfile;

public sealed record EditProfileCommandRequest(EditProfileDto Dto);
public  sealed record EditProfileCommandResponse(bool Success);
public record EditProfileCommand(EditProfileCommandRequest EditProfileDto):ICommand<EditProfileCommandResponse>;

public class EditProfileCommandHandler(IEditProfileService service,ICurrentUser currentUser):ICommandHandler<EditProfileCommand,EditProfileCommandResponse>
{
    public async Task<EditProfileCommandResponse> Handle(EditProfileCommand command, CancellationToken cancellationToken)
    {
        if (!currentUser.IsAuthenticated)
            throw new UnauthorizedAccessException();
        await service.EditProfile(currentUser.Subject, command.EditProfileDto.Dto,cancellationToken);
        return new EditProfileCommandResponse(true);
    }
}
