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
                async ([FromBody] EditProfileDto dto, [FromServices] ISender service,
                    CancellationToken ct) =>
                {
                    var command = new EditProfileCommand(new EditProfileCommandRequest(dto));
                    var result = await service.Send(command, ct);
                    var response = result.Adapt<EditProfileEndpointResponse>();
                    return Results.Ok(response);
                })  .WithName("EditUserProfile") 
            .Produces<EditProfileEndpointResponse>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status401Unauthorized)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .ProducesProblem(StatusCodes.Status409Conflict)
            .WithSummary("Edit current user profile")
            .WithDescription("Updates profile data of the currently authenticated user")
            .RequireAuthorization();
    }
}