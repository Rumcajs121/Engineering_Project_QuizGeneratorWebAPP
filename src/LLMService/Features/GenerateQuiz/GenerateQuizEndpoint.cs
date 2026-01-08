using Carter;
using LLMService.Commons.Models;
using LLMService.Features.CreateEmbendingWithChunk;
using Mapster;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
using StackExchange.Redis;

namespace LLMService.Features.GenerateQuiz;

public record GenerateQuizResponse(string JobId);

public class GenerateQuizEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("/generate", async ([FromBody] GenerateQuizRequest request, [FromServices] ISender sender) =>
            {
                var commandRequest = request.Adapt<GenerateQuizCommandRequest>();
                var command = new GenerateQuizCommand(commandRequest);
                var result = await sender.Send(command);
                var response = result.Adapt<GenerateQuizResponse>();
                return Results.Ok(response);
            }).WithName("GenerateQuiz")
            .Produces<GenerateQuizResponse>(StatusCodes.Status202Accepted)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status500InternalServerError)
            .WithSummary("Enqueue quiz generation")
            .WithDescription("""
                             Creates an asynchronous quiz generation job.

                             The request is accepted and processed in the background.
                             The response contains a JobId that can be used to track the job status
                             and retrieve the generated quiz once completed.
                             """);
        
        // ! DEVELOPING ENDPOINT !
        app.MapGet("/test/job/{jobId}", async ([FromRoute]string jobId,[FromServices] IConnectionMultiplexer redis) =>
        {
            var db = redis.GetDatabase();
            var key = $"ChunksCachejob:quiz:{jobId}";
            var json = await db.HashGetAsync(key, "data");
            var job = JsonConvert.DeserializeObject<QuizJob>(json!);
            var CountQuestion=job.Result.Questions.Count();
            Console.WriteLine(CountQuestion);
            return Results.Ok(job);
        });
    }
}