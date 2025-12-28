using Carter;
using Mapster;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using OpenTelemetry.Trace;
using UserService.Commons.Dto;

namespace UserService.Features.EditProfile;

public class EditProfileEndpoint:ICarterModule
{
    public record EditProfileEndpointRequest(string ExternalId,EditProfileDto Dto);
    public record EditProfileEndpointResponse(bool Success);
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPatch("/edit",
                async ([FromRoute] string Id, [FromBody] EditProfileDto dto, [FromServices] ISender service,
                    CancellationToken ct) =>
                {
                    var command = new EditProfileCommand(new EditProfileCommandRequest(Id, dto));
                    var result = await service.Send(command, ct);
                    var response = result.Adapt<EditProfileEndpointResponse>();
                    return Results.Ok(response);
                }).WithName("Edit domain profile")
            .Produces<EditProfileEndpointResponse>(StatusCodes.Status202Accepted)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .WithSummary("Edit domain profile")
            .WithDescription("Edit domain profile");
    }
}