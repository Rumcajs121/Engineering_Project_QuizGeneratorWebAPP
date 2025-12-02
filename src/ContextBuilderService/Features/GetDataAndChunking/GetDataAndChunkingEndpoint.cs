using Carter;
using Mapster;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace ContextBuilderService.Features.DataImport.GetDataAndChunking;

public record GetDataAndChunkingResponse(bool Success);
public class GetDataAndChunkingEndpoint:ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/chunking", async ([FromQuery] string request, [FromServices] ISender sender) =>
            {
            var command=new GetDataAndChunkingQuery(request);
            var result=await sender.Send(command);
            var response=result.Adapt<GetDataAndChunkingQueryResponse>();
            return Results.Ok(response);
        }).WithName("GetDataAndChunking")
        .DisableAntiforgery()
        .Produces<GetDataAndChunkingResponse>(StatusCodes.Status200OK)
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .WithSummary("GetDataAndChunking")
        .WithDescription("GetDataAndChunking");;
    }
}
