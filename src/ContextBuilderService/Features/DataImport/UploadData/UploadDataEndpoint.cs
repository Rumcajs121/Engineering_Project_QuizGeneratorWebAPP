using Carter;
using Mapster;
using MediatR;
using Microsoft.AspNetCore.Mvc;


namespace ContextBuilderService.ContextBuilder.UploadData;

public record UploadDataRequest(IFormFile File);
public record UploadDataResponse(bool Success);

public class UploadDataEndpoint:ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("/upload",async ([FromForm] UploadDataRequest request,[FromServices] ISender sender)=>
        {
            var command = new UploadDataCommand(request.File);
            var result=await sender.Send(command);
            var response=result.Adapt<UploadDataResponse>();
            return Results.Ok(response);
        }).WithName("CUploadData")
        .DisableAntiforgery()
        .Produces<UploadDataResponse>(StatusCodes.Status200OK)
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .WithSummary("UploadData")
        .WithDescription("UploadData");;
    }
}