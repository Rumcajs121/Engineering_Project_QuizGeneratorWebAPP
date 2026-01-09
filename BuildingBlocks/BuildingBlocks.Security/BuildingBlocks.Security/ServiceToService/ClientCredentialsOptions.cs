namespace BuildingBlocks.Security;

public class ClientCredentialsOptions
{
    public string TokenUrl { get; init; } = default!;
    public string ClientId { get; init; } = default!;
    public string ClientSecret { get; init; } = default!;
}