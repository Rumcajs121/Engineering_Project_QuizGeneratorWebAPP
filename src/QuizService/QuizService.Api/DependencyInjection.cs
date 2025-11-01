using QuizService.Domain.Abstraction;
using QuizService.Domain.Abstraction.Repository;
using QuizService.Infrastructure.Repository;

namespace QuizService.Api;

public static  class DependencyInjection
{
    public static IServiceCollection AddApiServices(this IServiceCollection services,IConfiguration configuration)
    {
        services.AddScoped<IQuizRepository,QuizRepository>();
        services.AddScoped<IUnitOfWork, EfUnitOfWork>();
        services.AddScoped<IQuizAttemptRepository,QuizAttemptRepository>();
        services.AddScoped<ITagRepository,TagRepository>();
        return services;
    }

    public static WebApplication UseApiService(this WebApplication app)
    {
        return app;
    }
}