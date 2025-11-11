using Carter;
using Mapster;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using QuizService.Application.Dtos;
using QuizService.Application.Quiz.Query.GetAllQuiz;

namespace QuizService.Api.Endpoints;


public record GetAllQuizResponse(List<ShortQuizDto> QuizzesShortInfo);
public class GetAllQuiz:ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/quiz", async ([FromServices] ISender sender) =>
        {
            var result = await sender.Send(new GetAllQuizQuery());
            var respone = result.Adapt<GetAllQuizResponse>();
            return Results.Ok(respone);
        }).WithName("GetAllQuiz")
        .Produces<CreateQuizResponse>(StatusCodes.Status200OK)
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .WithSummary("GetAllQuiz")
        .WithDescription("GetAllQuiz");
    }
}