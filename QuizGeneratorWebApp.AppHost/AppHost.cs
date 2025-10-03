var builder = DistributedApplication.CreateBuilder(args);

builder.AddProject<Projects.ContextBuilderService>("contextbuilderservice");

builder.AddProject<Projects.LLMService>("llmservice");

builder.AddProject<Projects.QuizGeneratorWebApp>("quizgeneratorwebapp");

builder.AddProject<Projects.QuizService_Api>("quizservice");

builder.AddProject<Projects.UserService>("userservice");

builder.AddProject<Projects.YarpGateway>("yarpgateway");

builder.Build().Run();
