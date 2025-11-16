using Carter;
using Mapster;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using QuizService.Application.Dtos;
using QuizService.Application.Quiz.Query.GetAttemptQuizById;

namespace QuizService.Api.Endpoints;

public record GetAttemptQuizByIdResponse(QuizAttemptViewDto AttemptByUser);
public class GetAttemptQuizById:ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/quiz/attempt/{id:guid}", async (Guid id,[FromServices] ISender sender) =>
        {
            var result = await sender.Send(new GetAttemptQuizByIdQuery(id));
            var response = result.Adapt<GetAttemptQuizByIdResponse>();
            return Results.Ok(response);
        }).WithName("GetAttemptQuizById")
        .Produces<GetAttemptQuizByIdResponse>(StatusCodes.Status200OK)
        .ProducesProblem(StatusCodes.Status404NotFound)
        .WithSummary("GetAttemptQuizById")
        .WithDescription("GetAttemptQuizById");
    }
}