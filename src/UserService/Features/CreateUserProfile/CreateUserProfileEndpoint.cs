using Carter;
using Mapster;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using UserService.Commons.Models;

namespace UserService.Features.CreateUserProfile;

public record CreateUserProfileRequest(UserDomain UserDomain);
public record CreateUserProfileResponse(bool Success);
public class CreateUserProfileEndpoint:ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("/create", async ([FromServices] ISender service,CancellationToken ct) =>
            {
                var result = await service.Send(new CreateUserProfileCommand(), ct);

                var response = result.Adapt<CreateUserProfileResponse>();
                return Results.Created("/create", response);
            }).WithName("CreateUserProfile")
            .Produces<CreateUserProfileResponse>()
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .WithSummary("Creates a new user profile")
            .WithDescription("Creates a new user profile based on the provided user domain data.")
            .RequireAuthorization();
    }
}