using Carter;
using Mapster;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using UserService.Commons.Dto;

namespace UserService.Features.GetUserProfile;

public record GetUserProfileEndpointResponse(ProfileDto Dto);
public class GetUserProfileEndpoint:ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/profile", async ([FromServices] ISender sender) =>
        {
            var result= await sender.Send(new GetUserProfileQuery());
            var response = result.Adapt<GetUserProfileEndpointResponse>();
            return Results.Ok(response);
        }).Produces<GetUserProfileEndpointResponse>(StatusCodes.Status200OK)
        .ProducesProblem(StatusCodes.Status401Unauthorized)
        .ProducesProblem(StatusCodes.Status404NotFound)
        .WithSummary("Get current user profile")
        .WithDescription("Returns profile data of the currently authenticated user")
        .RequireAuthorization();
    }
}