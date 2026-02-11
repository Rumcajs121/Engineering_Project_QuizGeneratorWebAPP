using BuildingBlocks.Exceptions.Handler;
using BuildingBlocks.Security;
using BuildingBlocks.Security.ClientToService.CurrentUser;
using UserService;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();
builder.Services.AddInfrastructure(builder.Configuration)
    .AddApplication(builder.Configuration)
    .AddApiService()
    .AddKeycloakJwtAuthentication(builder.Configuration)
    .AddKeycloakAuthorizationPolicies();
builder.Services.AddExceptionHandler<CustomExceptionHandler>();
builder.Services.AddCurrentUser();
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
app.UseExceptionHandler(options=>{});
app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.UseApiService();
app.Run();