using Carter;
using Mapster;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace LLMService.Features.CreateEmbendingWithChunk;

public class CreateEmbeddingWithChunkEndpoint:ICarterModule
{
    public record CreateEmbeddingWithChunkResponse(bool Success);
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("/embedding/{documentId:guid}", async (Guid documentId, [FromServices] ISender sender) =>
        {
            var command=new CreateEmbeddingWithChunkCommand(documentId);
            var result=await sender.Send(command);
            var response = result.Adapt<CreateEmbeddingWithChunkResponse>();
            return Results.Ok(response);
        }).WithName("CreateEmbeddingWithChunk")
        .Produces<CreateEmbeddingWithChunkResponse>()
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .WithSummary("CreateEmbeddingWithChunk")
        .WithDescription("CreateEmbeddingWithChunk");
    }
}