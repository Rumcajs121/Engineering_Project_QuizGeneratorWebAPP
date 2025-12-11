using Carter;

namespace LLMService.Features.GenerateQuiz;

public class GenerateQuizEndpoint:ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("/generate", () => "OK"); 
    }
}