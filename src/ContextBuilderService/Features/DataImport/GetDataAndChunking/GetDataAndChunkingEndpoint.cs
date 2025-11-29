using Carter;

namespace ContextBuilderService.Features.DataImport.GetDataAndChunking;

public class GetDataAndChunkingEndpoint:ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/test", () =>
        {
            return Results.Ok("Hello World");
        });
    }
}
