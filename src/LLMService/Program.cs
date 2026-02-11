using BuildingBlocks.Exceptions.Handler;
using BuildingBlocks.Security;
using BuildingBlocks.Security.ClientToService.CurrentUser;
using BuildingBlocks.Security.ServiceToService;
using LLMService;


var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();


builder.Services
    .AddLLM(builder.Configuration)
    .AddApiService()
    .AddInfrastructure(builder.Configuration)
    .AddApplication(builder.Configuration)
    .AddKeycloakJwtAuthentication(builder.Configuration)
    .AddKeycloakAuthorizationPolicies();
builder.Services.AddEndpointsApiExplorer(); 
builder.Services.AddSwaggerGen();

builder.Services.AddKeycloakServiceToServiceAuthentication(builder.Configuration);
builder.Services.AddHttpClient("llmtoquizcomunications", client =>
{
    client.BaseAddress = new Uri("https://localhost:6033/"); 
    client.Timeout = TimeSpan.FromSeconds(30);
}).AddKeycloakServiceToServiceAuthentication();
builder.Services.AddCurrentUser();
builder.Services.AddExceptionHandler<CustomExceptionHandler>();
var app = builder.Build();

app.MapDefaultEndpoints();
// Configure the HTTP parameter pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}


app.UseHttpsRedirection();
app.UseExceptionHandler(options=>{});
app.UseAuthentication();
app.UseAuthorization();
app.UseApiService();
await app.RunAsync();
