using Carter;
using Mapster;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using QuizService.Application.Dtos;
using QuizService.Application.Quiz.Command.QuizCreate;

namespace QuizService.Api.Endpoints;
public record CreateQuizRequest(CreateQuizDto CreateQuizDto);
public record CreateQuizResponse(Guid Id);

public class CreateQuiz:ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("/quiz", async ([FromBody] CreateQuizRequest request, [FromServices] ISender sender) =>
        {
            var command = request.Adapt<QuizCreateCommand>();
            var result = await sender.Send(command);
            var response = result.Adapt<CreateQuizResponse>();
            return Results.Created($"/quiz/{response.Id}", response);
        }).WithName("CreateQuiz")
        .Produces<CreateQuizResponse>(StatusCodes.Status201Created)
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .WithSummary("CreateQuiz")
        .WithDescription("CreateQuiz");
    }
}
