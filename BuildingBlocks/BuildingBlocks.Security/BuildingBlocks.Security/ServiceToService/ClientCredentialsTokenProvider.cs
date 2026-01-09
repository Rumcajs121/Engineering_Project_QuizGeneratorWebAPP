using System.Text.Json;
using Microsoft.Extensions.Options;

namespace BuildingBlocks.Security;

public sealed class ClientCredentialsTokenProvider( HttpClient http,
    IOptionsMonitor<ClientCredentialsOptions> options)
{   
    private readonly SemaphoreSlim _lock = new(1, 1);
    private string? _token;
    private DateTimeOffset _expiresAtUtc = DateTimeOffset.MinValue;
    public async Task<string> GetTokenAsync(CancellationToken ct = default)
    {
        if (!string.IsNullOrWhiteSpace(_token) && DateTimeOffset.UtcNow < _expiresAtUtc)
        {
            return _token;
        }

        await _lock.WaitAsync(ct);
        try
        {
            if (!string.IsNullOrWhiteSpace(_token) && DateTimeOffset.UtcNow < _expiresAtUtc)
            {
                return _token;
            }

            var o = options.CurrentValue;
            if (string.IsNullOrWhiteSpace(o.TokenUrl))
                throw new InvalidOperationException("KeycloakServiceAuthentication:TokenUrl missing");
            if (string.IsNullOrWhiteSpace(o.ClientId))
                throw new InvalidOperationException("KeycloakServiceAuthentication:ClientId missing");
            if (string.IsNullOrWhiteSpace(o.ClientSecret))
                throw new InvalidOperationException("KeycloakServiceAuthentication:ClientSecret missing");
            using var form = new FormUrlEncodedContent(new Dictionary<string, string>{
                ["grant_type"] = "client_credentials",
                ["client_id"] = o.ClientId,
                ["client_secret"] = o.ClientSecret
            });
            using var response=await http.PostAsync(o.TokenUrl, form,ct);
            var json = await response.Content.ReadAsStringAsync(ct);
            if (!response.IsSuccessStatusCode)
                throw new InvalidOperationException($"Keycloak token request failed: {(int)response.StatusCode}. Body: {json}");
            using var doc = JsonDocument.Parse(json);
            _token = doc.RootElement.GetProperty("access_token").GetString()
                     ?? throw new InvalidOperationException("Keycloak response missing access_token");
            var expiresIn = doc.RootElement.TryGetProperty("expires_in", out var e) ? e.GetInt32() : 60;
            _expiresAtUtc = DateTimeOffset.UtcNow.AddSeconds(Math.Max(10, expiresIn - 15));
            return _token;
        }
        finally
        {
            _lock.Release();
        }
    }
}