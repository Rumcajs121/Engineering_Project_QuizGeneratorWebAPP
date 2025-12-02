using System.Reflection;
using Carter;
using LLMService.Features.CreateEmbendingWithChunk;
using LLMService.Features.GenerateQuiz;

namespace LLMService;

public static class Dependencyinjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services,IConfiguration configuration)
    {
        services.AddStackExchangeRedisCache(options =>
        {
            options.Configuration = configuration.GetConnectionString("Redis");
            options.InstanceName = "ChunksCache";
        });
        return services;
    }
    public static IServiceCollection AddApplication(this IServiceCollection services,IConfiguration configuration)
    {
        services.AddMediatR(cfg =>
        {
            cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());
        });
        return services;
    }
    public static IServiceCollection AddApiService(this IServiceCollection services)
    {
        services.AddCarter(configurator: c =>
        {
            c.WithModule<CreateEmbendingWithChunkEndpoint>();
            c.WithModule<GenerateQuizEndpoint>();
        });
        return services;
        return services;
    }
    public static WebApplication UseApiService(this WebApplication app)
    {
        app.MapCarter();
        return app;
    }
}