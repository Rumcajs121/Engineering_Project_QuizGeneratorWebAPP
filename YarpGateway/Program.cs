using BuildingBlocks.Security;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();
//Cors for Blazor SPA
// builder.Services.AddCors(options =>
// {
//     options.AddPolicy("GatewayCorsPolicy", policy =>
//     {
//         policy.WithOrigins("https://localhost:7107")
//             .AllowAnyMethod()
//             .WithHeaders("Authorization")
//             .SetPreflightMaxAge(TimeSpan.FromMinutes(10));
//     });
// });
builder.Services.AddKeycloakJwtAuthentication(builder.Configuration)
    .AddKeycloakAuthorizationPolicies();
builder.Services.AddReverseProxy()
    .LoadFromConfig(builder.Configuration.GetSection("ReverseProxy"));

var app = builder.Build();
app.UseCors("GatewayCorsPolicy");
app.MapDefaultEndpoints();
app.UseAuthentication();
app.UseAuthorization();
app.MapReverseProxy().RequireAuthorization();
app.Run();