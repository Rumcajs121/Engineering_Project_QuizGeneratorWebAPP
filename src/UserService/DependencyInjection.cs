using System.Reflection;
using Carter;
using Microsoft.EntityFrameworkCore;
using UserService.Commons.Dto;
using UserService.Features.CheckPermissions;
using UserService.Features.CreateUserProfile;
using UserService.Infrastructure;

namespace UserService;

public static class DependencyInjection
{
     public static IServiceCollection AddInfrastructure(this IServiceCollection services,IConfiguration configuration)
    {
        services.AddDbContext<UserDbContext>(
            options=>options.UseSqlServer(configuration.GetConnectionString("Database")));
        services.AddScoped<IDataRepository, DataRepository>();
        services.AddScoped<ICurrentUser, CurrentUser>();
         return services;
    }
    public static IServiceCollection AddApplication(this IServiceCollection services,IConfiguration configuration)
    {
        services.AddMediatR(cfg =>
        {
            cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());
        });
        services.AddScoped<ICheckPermissionsService, CheckPermissionsService>();
        services.AddScoped<ICreateUserProfileService,CreateUserProfileService>();
        return services;
    }
    public static IServiceCollection AddApiService(this IServiceCollection services)
    {
        services.AddCarter(configurator: c =>
        {
            c.WithModule<CheckPermissionsEndpoint>();
            c.WithModule<CreateUserProfileEndpoint>();
        });
        
        return services;
    }
    public static WebApplication UseApiService(this WebApplication app)
    {
        app.MapCarter();
        return app;
    }
}