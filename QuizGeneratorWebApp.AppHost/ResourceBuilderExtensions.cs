using System.Diagnostics;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace QuizGeneratorWebApp.AppHost;

public static class ResourceBuilderExtensions
{
    // var name = "swagger-ui-docs";
    // var displayName = "Swagger UI Documentation";
    // var openApiUiPath = "swagger";
    internal static IResourceBuilder<T> WithSwaggerUI<T>(this IResourceBuilder<T> builder)
        where T : IResourceWithEndpoints
    {
        return builder.WithOpenApiDocs("swagger-ui-docs","Swagger UI Documentation","swagger");
    }
    private static IResourceBuilder<T> WithOpenApiDocs<T>(this IResourceBuilder<T> builder,string name,string displayName,string openApiUiPath ) 
        where T:IResourceWithEndpoints
    {
        return builder.WithCommand(name,displayName,executeCommand:
            async _ =>
            {
                try
                {
                    var endpoint = builder.GetEndpoint("https");
                    var url = $"{endpoint.Url}/{openApiUiPath}";
                    Process.Start(new ProcessStartInfo(url) { UseShellExecute = true });

                    return new ExecuteCommandResult { Success = true };
                }
                catch(Exception ex)
                {
                    return new ExecuteCommandResult { Success = false, ErrorMessage = ex.ToString() };
                }
        
            },
            updateState: context=>context.ResourceSnapshot.HealthStatus==HealthStatus.Healthy ? ResourceCommandState.Enabled : ResourceCommandState.Disabled, iconName:"Document", iconVariant: IconVariant.Filled
        );
    }
}