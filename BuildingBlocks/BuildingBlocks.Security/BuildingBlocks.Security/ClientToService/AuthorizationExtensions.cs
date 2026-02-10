using System.Runtime.InteropServices.ComTypes;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.DependencyInjection;

namespace BuildingBlocks.Security;

public static class AuthorizationExtensions
{
    public static IServiceCollection AddKeycloakAuthorizationPolicies(
        this IServiceCollection services)
    {
        services.AddAuthorization(options =>
        {
            options.FallbackPolicy=new AuthorizationPolicyBuilder().RequireAuthenticatedUser().Build();
            options.AddPolicy("Public",p=>p.RequireAssertion(_=>true));
        });
        return services;
    }
}