using Carter;
using Mapster;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace LLMService.Features.CreateEmbendingWithChunk;

public class CreateEmbendingWithChunkEndpoint:ICarterModule
{
    public record CreateEmbendingWithChunkResponse(bool Success);
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("/embending/{request:guid}", async (Guid documentId, [FromServices] ISender sender) =>
        {
            var command=new CreateEmbendingWithChunkCommand(documentId);
            var result=await sender.Send(command);
            var response = result.Adapt<CreateEmbendingWithChunkResponse>();
            return Results.Ok(response);
        }).WithName("CreateEmbendingWithChunk")
        .DisableAntiforgery()
        .Produces<CreateEmbendingWithChunkResponse>(StatusCodes.Status201Created)
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .WithSummary("CreateEmbendingWithChunk")
        .WithDescription("CreateEmbendingWithChunk");;;
    }
}