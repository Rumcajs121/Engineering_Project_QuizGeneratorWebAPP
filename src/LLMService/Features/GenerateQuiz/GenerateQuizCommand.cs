using BuildingBlocks.CQRS;
using BuildingBlocks.Security.ClientToService.CurrentUser;
using LLMService.Commons.Models;
using LLMService.Infrastructure.Redis;
using Newtonsoft.Json;

namespace LLMService.Features.GenerateQuiz;
public record GenerateQuizCommandResponse(string JobId);
public record GenerateQuizCommandRequest(int K, int CountQuestion,string Question, IReadOnlyList<Guid> DocumentIds);
public record GenerateQuizCommand(GenerateQuizCommandRequest Request) : ICommand<GenerateQuizCommandResponse>;
public class GenerateQuizCommandHandler(IGenerateQuizService service,ICurrentUser currentUser):ICommandHandler<GenerateQuizCommand, GenerateQuizCommandResponse>
{
    public async Task<GenerateQuizCommandResponse> Handle(GenerateQuizCommand command, CancellationToken cancellationToken)
    {
        
        var jobId=await service.CreateJobAndAddQueue(command.Request.K,command.Request.CountQuestion,command.Request.Question,command.Request.DocumentIds,cancellationToken);
        return new GenerateQuizCommandResponse(jobId);
    }
}