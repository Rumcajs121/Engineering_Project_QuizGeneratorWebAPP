using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace BuildingBlocks.Security.ServiceToService;

public static class ServiceToServiceAuthenticationExtensions
{
    public static IServiceCollection AddKeycloakServiceToServiceAuthentication(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.Configure<ClientCredentialsOptions>(
            configuration.GetSection("KeycloakServiceAuthentication"));
        services.AddHttpClient<ClientCredentialsTokenProvider>();
        services.AddTransient<ClientCredentialsBearerHandler>();
        return services;
    }

    public static IHttpClientBuilder AddKeycloakServiceToServiceAuthentication(this IHttpClientBuilder builder)=>
        builder.AddHttpMessageHandler<ClientCredentialsBearerHandler>();
}