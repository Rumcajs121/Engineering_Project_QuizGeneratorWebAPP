namespace QuizService.Api;

public static  class DependencyInjection
{
    public static IServiceCollection AddApiServices(this IServiceCollection services,IConfiguration configuration)
    {
        
        return services;
    }

    public static WebApplication UseApiService(this WebApplication app)
    {
        return app;
    }
}