using BuildingBlocks.Exceptions.Handler;
using Carter;
using QuizService.Api.Endpoints;
using QuizService.Application.UseCases;
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
        services.AddScoped<IQuizAttemptService,QuizAttemptService>();
        services.AddScoped<IQuizService, Application.UseCases.QuizService>();

        services.AddCarter(configurator: c =>
        {
            c.WithModule<CreateQuiz>();
            c.WithModule<GetAllQuiz>();
            c.WithModule<GetAttemptQuizById>();
            c.WithModule<GetQuizById>();
            c.WithModule<QuizAttemptCreate>();
            c.WithModule<SubmitQuiz>();
        });
        
        return services;
    }

    public static WebApplication UseApiService(this WebApplication app)
    {
        app.MapCarter();
        return app;
    }
}