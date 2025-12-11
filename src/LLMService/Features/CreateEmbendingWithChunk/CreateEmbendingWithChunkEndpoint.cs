using Carter;

namespace LLMService.Features.CreateEmbendingWithChunk;

public class CreateEmbendingWithChunkEndpoint:ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("/embending", () => "OK"); 
    }
}