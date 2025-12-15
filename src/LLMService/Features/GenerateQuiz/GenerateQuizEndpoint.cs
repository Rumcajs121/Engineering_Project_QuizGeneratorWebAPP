using Carter;
using LLMService.Commons.Models;
using LLMService.Features.CreateEmbendingWithChunk;
using Mapster;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace LLMService.Features.GenerateQuiz;

public record GenerateQuizRequest(int K, int CountQuestion, string Question,List<Guid> DocumentIds);

public record GenerateQuizResponse(LlmQuiz Quiz);

public class GenerateQuizEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("/generate", async ([FromBody] GenerateQuizRequest request, [FromServices] ISender sender) =>
            {
                var commandRequest= request.Adapt<GenerateQuizCommandRequest>();
                var command =new GenerateQuizCommand(commandRequest);
                var result = await sender.Send(command);
                var response = result.Adapt<GenerateQuizResponse>();
                return Results.Ok(response);
            }).WithName("GenerateQuiz")
            .Produces<GenerateQuizResponse>()
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .WithSummary("GenerateQuiz")
            .WithDescription("GenerateQuiz");
    }
}