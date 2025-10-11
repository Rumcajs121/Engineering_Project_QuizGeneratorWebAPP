using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using QuizService.Infrastructure.Data;

namespace QuizService.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructureService(this IServiceCollection services,IConfiguration configuration)
    {
        services.AddDbContext<QuizDbCOntext>(
            options=>options.UseSqlServer(configuration.GetConnectionString("ConnectionStrings")));
        return services;
    }
}