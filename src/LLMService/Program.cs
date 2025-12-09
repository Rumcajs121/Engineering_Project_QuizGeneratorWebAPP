using LLMService;
using Microsoft.Agents.AI.Hosting.AGUI.AspNetCore;
using Microsoft.Extensions.AI;


var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();
builder.Services.AddAGUI();

builder.Services
    .AddApiService()
    .AddInfrastructure(builder.Configuration)
    .AddApplication(builder.Configuration);
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
app.UseApiService();
var chatClient = app.Services.GetRequiredService<IChatClient>();
var agent = chatClient.CreateAIAgent(
    name: "QuizGeneratorAgent",
    instructions: """
                  Jesteś asystentem do tworzenia quizów edukacyjnych. 
                  Generujesz pytania w formacie JSON. 
                  Odpowiadasz po polsku.
                  """);

app.MapAGUI("/agent", agent);
app.Run();
