using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using QuizService.Infrastructure.Data;
using QuizService.Infrastructure.Interceptors;

namespace QuizService.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructureService(this IServiceCollection services,IConfiguration configuration)
    {
        services.AddScoped<ISaveChangesInterceptor, AuditableEntityInterceptors>();
        services.AddDbContext<QuizDbContext>((sp, options)=>
            {
                options.UseSqlServer(configuration.GetConnectionString("Database"));
                options.AddInterceptors(sp.GetRequiredService<AuditableEntityInterceptors>());
            });
        return services;
    }
}