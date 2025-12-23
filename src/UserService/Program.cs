using BuildingBlocks.Security;
using UserService;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();
builder.Services.AddInfrastructure(builder.Configuration)
    .AddApplication(builder.Configuration)
    .AddApiService()
    .AddKeycloakJwtAuthentication(builder.Configuration)
    .AddKeycloakAuthorizationPolicies()
    .AddHttpContextAccessor();
builder.Services.AddEndpointsApiExplorer(); 

builder.Services.AddSwaggerGen();

var app = builder.Build();

app.MapDefaultEndpoints();
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.UseApiService();
app.Run();