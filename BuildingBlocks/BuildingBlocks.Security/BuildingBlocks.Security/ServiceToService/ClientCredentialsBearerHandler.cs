using System.Net.Http.Headers;

namespace BuildingBlocks.Security.ServiceToService;

public class ClientCredentialsBearerHandler(ClientCredentialsTokenProvider provider):DelegatingHandler
{
    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken ct)
    {
        var token = await provider.GetTokenAsync(ct);
        request.Headers.Authorization=new AuthenticationHeaderValue("Bearer", token);
        return await base.SendAsync(request, ct);
    }
}