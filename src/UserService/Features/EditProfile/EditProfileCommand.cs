using BuildingBlocks.CQRS;
using BuildingBlocks.Security.ClientToService.CurrentUser;
using UserService.Commons.Dto;


namespace UserService.Features.EditProfile;

public sealed record EditProfileCommandRequest(EditProfileDto Dto);
public  sealed record EditProfileCommandResponse(bool Success);
public record EditProfileCommand(EditProfileCommandRequest EditProfileDto):ICommand<EditProfileCommandResponse>;

public class EditProfileCommandHandler(IEditProfileService service):ICommandHandler<EditProfileCommand,EditProfileCommandResponse>
{
    public async Task<EditProfileCommandResponse> Handle(EditProfileCommand command, CancellationToken cancellationToken)
    {
        await service.EditProfile(command.EditProfileDto.Dto,cancellationToken);
        return new EditProfileCommandResponse(true);
    }
}
