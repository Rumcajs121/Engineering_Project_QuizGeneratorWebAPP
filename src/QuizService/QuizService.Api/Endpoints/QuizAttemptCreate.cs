using Carter;
using Mapster;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using QuizService.Application.Dtos;
using QuizService.Application.Quiz.Command.QuizAttemptCreate;
using QuizService.Application.Quiz.Command.QuizCreate;

namespace QuizService.Api.Endpoints;
public record QuizAttemptCreateRequest(Guid Id);
public record QuizAttemptCreateResponse(QuizAttemptViewDto viewDto);

public class QuizAttemptCreate:ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("/quiz/attempt", async ([FromBody] QuizAttemptCreateRequest request, ISender sender) =>
            {
                var command = request.Adapt<QuizAttemptCreateCommand>();
                var result = await sender.Send(command);
                var response = result.Adapt<QuizAttemptCreateResponse>();
                return Results.Created($"/quiz/{response.viewDto.QuizId}", response);
            }).WithName("QuizAttemptCreate")
            .Produces<QuizAttemptCreateResponse>(StatusCodes.Status201Created)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .WithSummary("QuizAttemptCreate")
            .WithDescription("QuizAttemptCreate");
    }
}