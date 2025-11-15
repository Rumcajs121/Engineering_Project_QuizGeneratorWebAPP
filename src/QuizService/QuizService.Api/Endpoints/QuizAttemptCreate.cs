using Carter;
using Mapster;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using QuizService.Application.Dtos;
using QuizService.Application.Quiz.Command.QuizAttemptCreate;
using QuizService.Application.Quiz.Command.QuizCreate;

namespace QuizService.Api.Endpoints;

public record QuizAttemptCreateResponse(QuizAttemptViewDto ViewDto);

public class QuizAttemptCreate:ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("/quiz/attempt/{id:guid}", async (Guid id, ISender sender) =>
            {
                var result = await sender.Send(new  QuizAttemptCreateCommand(id));
                var response = result.Adapt<QuizAttemptCreateResponse>();
                return Results.Created($"/quiz/{response.ViewDto.QuizId}", response);
            }).WithName("QuizAttemptCreate")
            .Produces<QuizAttemptCreateResponse>(StatusCodes.Status201Created)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .WithSummary("QuizAttemptCreate")
            .WithDescription("QuizAttemptCreate");
    }
}