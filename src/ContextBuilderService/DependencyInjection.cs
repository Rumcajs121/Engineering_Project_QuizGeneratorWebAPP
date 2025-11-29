using System.Reflection;
using Carter;
using ContextBuilderService.ContextBuilder.UploadData;
using ContextBuilderService.Domain.Repository;
using ContextBuilderService.Features.DataImport.GetDataAndChunking;
using ContextBuilderService.Features.DataImport.UploadData;
using ContextBuilderService.Infrastructure.DataImport.Repositories;

namespace ContextBuilderService;

public static class DependencyInjection
{
    public static IServiceCollection AddDistributedCache(this IServiceCollection services,IConfiguration configuration)
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
        services.AddScoped<IRepository, DataRepository>();
        services.AddScoped<IUploadDataService, UploadDataService>();
        services.AddScoped<IGetDataAndChunkingService, GetDataAndChunkingService>();
        
        services.AddCarter(configurator: c =>
        {
            c.WithModule<GetDataAndChunkingEndpoint>();
            c.WithModule<UploadDataEndpoint>();
        });
        return services;
    }

    public static WebApplication UseApiService(this WebApplication app)
    {
        app.MapCarter();
        return app;
    }
}
