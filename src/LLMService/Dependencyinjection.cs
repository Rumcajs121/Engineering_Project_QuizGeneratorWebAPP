using System.Reflection;
using Carter;
using LLMService.Features.CreateEmbendingWithChunk;
using LLMService.Features.GenerateQuiz;
using LLMService.Infrastructure.DistributedCache;
using LLMService.Infrastructure.LLMProvider;
using LLMService.Infrastructure.VectorStore;
using Microsoft.Extensions.AI;
using Qdrant.Client;

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
        services.AddSingleton(_ =>
            new QdrantClient(new Uri("http://localhost:6334")));
        services.AddScoped<IVectorDataRepository,VectorDataRepository>();
        services.AddScoped<ICacheDataRepository, CacheDataRepository>();
        return services;
    }
    public static IServiceCollection AddApplication(this IServiceCollection services,IConfiguration configuration)
    {
        services.AddMediatR(cfg =>
        {
            cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());
        });
        services.AddScoped<ICreateEmbeddingWithChunkService, CreateEmbeddingWithChunkService>();
        services.AddScoped<IGenerateQuizService, GenerateQuizService>();
        return services;
    }
    public static IServiceCollection AddApiService(this IServiceCollection services)
    {
        services.AddCarter(configurator: c =>
        {
            c.WithModule<CreateEmbeddingWithChunkEndpoint>();
            c.WithModule<GenerateQuizEndpoint>();
        });
        
        
        

        return services;
    }

    public static IServiceCollection AddLLM(this IServiceCollection services,IConfiguration configuration)
    {

        services.AddSingleton<ChatModelClient>();
        services.AddSingleton<EmbeddingModelClient>();
        
        services.AddSingleton<IChatClient>(sp =>
            sp.GetRequiredService<ChatModelClient>());

        services.AddSingleton<IEmbeddingGenerator<string, Embedding<float>>>(sp =>
            sp.GetRequiredService<EmbeddingModelClient>());
        return services;
    }
    public static WebApplication UseApiService(this WebApplication app)
    {
        app.MapCarter();
        return app;
    }
}