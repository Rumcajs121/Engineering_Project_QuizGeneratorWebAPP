using Carter;
using Mapster;
using MediatR;
using QuizService.Application.Dtos;
using QuizService.Application.Quiz.Query.GetQuizById;

namespace QuizService.Api.Endpoints;

public record GetQuizByIdResponse(QuizDto QuizDto);
public class GetQuizById:ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/quiz/{id:guid}", async (Guid id,ISender sender) =>
            {
                var result = await sender.Send(new GetQuizByIdQuery(id));
                var response = result.Adapt<GetQuizByIdResponse>();
                return Results.Ok(response);
            }).WithName("GetQuizById")
            .Produces<GetQuizByIdResponse>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .WithSummary("GetQuizById")
            .WithDescription("GetQuizById");
    }
}