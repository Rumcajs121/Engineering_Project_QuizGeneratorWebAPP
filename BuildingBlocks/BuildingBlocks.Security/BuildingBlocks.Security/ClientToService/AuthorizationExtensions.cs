using Microsoft.Extensions.DependencyInjection;

namespace BuildingBlocks.Security;

public static class AuthorizationExtensions
{
    public static IServiceCollection AddKeycloakAuthorizationPolicies(
        this IServiceCollection services)
    {
        services.AddAuthorization(options =>
        {
            options.AddPolicy("User", policy =>
                policy.RequireAuthenticatedUser()
                    .RequireRole("user"));

            options.AddPolicy("Admin", policy =>
                policy.RequireAuthenticatedUser()
                    .RequireRole("admin"));

            options.AddPolicy("Operator", policy =>
                policy.RequireAuthenticatedUser()
                    .RequireRole("operator"));
        });
        return services;
    }
}