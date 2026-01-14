using Carter;
using Mapster;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace UserService.Features.CheckPermissions;

public record CheckPermissionsQueryEndpointResponse(string Privelage);
public class CheckPermissionsEndpoint:ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        
        app.MapGet("/permission", async ([FromServices] ISender sender) =>
            {
                var command=new CheckPermissionsQuery();
                var result=await sender.Send(command);
                var response = result.Adapt<CheckPermissionsQueryEndpointResponse>();
                return Results.Ok(response);
            }).WithName("CheckPermissions")
            .Produces<CheckPermissionsQueryEndpointResponse>()
            .ProducesProblem(StatusCodes.Status401Unauthorized)
            .WithSummary("CheckPermissions")
            .WithDescription("CheckPermissions")
            .RequireAuthorization();
    }
}