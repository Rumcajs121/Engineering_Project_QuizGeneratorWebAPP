using Carter;
using Mapster;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace UserService.Features.CheckPermissions;

public record CheckPermissionsQueryEndpointRequest(string ExternalId,string Privilege);
public record CheckPermissionsQueryEndpointResponse(bool Success);
public class CheckPermissionsEndpoint:ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        
        app.MapGet("/permission", async ([AsParameters]CheckPermissionsQueryEndpointRequest request, [FromServices] ISender sender) =>
            {
                var commandRequest = request.Adapt<CheckPermissionsQueryRequest>();
                var command=new CheckPermissionsQuery(commandRequest);
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