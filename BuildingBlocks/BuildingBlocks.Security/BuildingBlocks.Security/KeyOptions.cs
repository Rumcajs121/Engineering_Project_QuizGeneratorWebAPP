namespace BuildingBlocks.Security;

public class KeyOptions
{
    public string Authority { get; init; } = default!;
    public string Audience { get; init; } = default!;
    public string ClientId { get; init; } = default!;
    public bool RequireHttpsMetadata { get; init; } = false;
    public TimeSpan ClockSkew { get; init; } = TimeSpan.FromMinutes(2);
}