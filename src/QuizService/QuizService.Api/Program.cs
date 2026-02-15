

using BuildingBlocks.Exceptions.Handler;
using BuildingBlocks.Security;
using BuildingBlocks.Security.ClientToService.CurrentUser;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddEndpointsApiExplorer(); 
builder.Services.AddSwaggerGen();
builder.Services
    .AddApiServices(builder.Configuration)
    .AddApplicationService(builder.Configuration)
    .AddInfrastructureService(builder.Configuration);
builder.Services.AddKeycloakJwtAuthentication(builder.Configuration)
    .AddKeycloakAuthorizationPolicies()
    .AddCurrentUser();
builder.Services.AddExceptionHandler<CustomExceptionHandler>();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseAuthentication();
app.UseAuthorization();
app.UseExceptionHandler(options=>{});
app.UseHttpsRedirection();
app.UseApiService();


app.Run();
