using Microsoft.EntityFrameworkCore;
using UserService.Infrastructure;

namespace UserService;

public static class DependencyInjection
{
     public static IServiceCollection AddInfrastructure(this IServiceCollection services,IConfiguration configuration)
    {
        services.AddDbContext<UserDbContext>(
            options=>options.UseSqlServer(configuration.GetConnectionString("Database")));

         return services;
    }
    public static IServiceCollection AddApplication(this IServiceCollection services,IConfiguration configuration)
    {
        return services;
    }
    public static IServiceCollection AddApiService(this IServiceCollection services)
    {
        return services;
    }
    public static WebApplication UseApiService(this WebApplication app)
    {

        return app;
    }
}