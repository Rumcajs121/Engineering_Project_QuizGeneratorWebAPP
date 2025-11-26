using System.Diagnostics;
using System.Runtime.InteropServices.ComTypes;
using k8s.ClientSets;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using QuizGeneratorWebApp.AppHost;

var builder = DistributedApplication.CreateBuilder(args);

//---BACKENDY---

var ctxBuilderSvc=builder.AddProject<Projects.ContextBuilderService>("contextbuilderservice");
var llmService=builder.AddProject<Projects.LLMService>("llmservice").WithSwaggerUI();
var quizService = builder.AddProject<Projects.QuizService_Api>("quizservice")
    .WithSwaggerUI();

var userService =builder.AddProject<Projects.UserService>("userservice")
    .WithSwaggerUI();
//--GATEWAY--
var yarp=builder.AddProject<Projects.YarpGateway>("yarpgateway").WithExternalHttpEndpoints()
    .WithReference(userService).WaitFor(userService)
    .WithReference(quizService).WaitFor(quizService)
    .WithReference(llmService).WaitFor(llmService)
    .WithReference(ctxBuilderSvc).WaitFor(ctxBuilderSvc);;
//--Frontend--
builder.AddProject<Projects.QuizGeneratorWebApp>("quizgeneratorwebapp")
    .WithExternalHttpEndpoints()
    .WithReference(yarp)
    .WaitFor(yarp);


builder.Build().Run();



