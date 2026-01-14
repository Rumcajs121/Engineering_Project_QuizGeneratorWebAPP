using Microsoft.Extensions.DependencyInjection;

namespace BuildingBlocks.Security.ClientToService.CurrentUser;

public static class AddCurrentUserExtensions
{
    public static IServiceCollection AddCurrentUser(this IServiceCollection services)
    {
        services.AddHttpContextAccessor();
        services.AddScoped<ICurrentUser, CurrentUser>();
        return services;
    }
}