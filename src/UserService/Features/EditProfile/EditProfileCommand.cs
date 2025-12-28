using BuildingBlocks.CQRS;
using UserService.Commons.Dto;
using UserService.Commons.Models;

namespace UserService.Features.EditProfile;

//TODO Check abstract record;
public record EditProfileCommandRequest(string ExternalId,EditProfileDto Dto);
public record EditProfileCommandResponse(bool Success);
public record EditProfileCommand(EditProfileCommandRequest EditProfileDto):ICommand<EditProfileCommandResponse>;

public class EditProfileCommandHandler(IEditProfileService service):ICommandHandler<EditProfileCommand,EditProfileCommandResponse>
{
    public async Task<EditProfileCommandResponse> Handle(EditProfileCommand command, CancellationToken cancellationToken)
    {
        await service.EditProfile(command.EditProfileDto.ExternalId, command.EditProfileDto.Dto,cancellationToken);
        return new EditProfileCommandResponse(true);
    }
}
