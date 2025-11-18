using Carter;
using Mapster;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using QuizService.Application.Dtos;
using QuizService.Application.Quiz.Command.SubmitQuizCommand;

namespace QuizService.Api.Endpoints;

public record SubmitQuizRequest(SubmitQuizAnswersDto Dto);
public record SubmitQuizResponse(int Score);
public class SubmitQuiz:ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("/quiz/submit", async ([FromBody] SubmitQuizRequest request, [FromServices] ISender sender) =>
            {
                var command = request.Adapt<SubmitQuizCommand>();
                var result = await sender.Send(command);
                var response = result.Adapt<SubmitQuizResponse>();
                return Results.Created($"Your Score:{response.Score}", response);
            }).WithName("SubmitQuiz")
            .Produces<SubmitQuizResponse>(StatusCodes.Status201Created)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .WithSummary("SubmitQuiz")
            .WithDescription("SubmitQuiz");
    }
}