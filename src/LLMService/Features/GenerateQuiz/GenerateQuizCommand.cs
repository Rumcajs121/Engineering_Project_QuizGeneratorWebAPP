using BuildingBlocks.CQRS;
using LLMService.Commons.Models;
using LLMService.Infrastructure.Redis;
using Newtonsoft.Json;

namespace LLMService.Features.GenerateQuiz;
public record GenerateQuizCommandResponse(string JobId);
public record GenerateQuizCommandRequest(int K, int CountQuestion,string Question, IReadOnlyList<Guid> DocumentIds);
public record GenerateQuizCommand(GenerateQuizCommandRequest Request) : ICommand<GenerateQuizCommandResponse>;
public class GenerateQuizCommandHandler(IRedisDataRepository repository):ICommandHandler<GenerateQuizCommand, GenerateQuizCommandResponse>
{
    public async Task<GenerateQuizCommandResponse> Handle(GenerateQuizCommand command, CancellationToken cancellationToken)
    {

        var jobId = await repository.CreateJobAsync(new GenerateQuizRequest(
                command.Request.K,
                command.Request.CountQuestion,
                command.Request.Question,
                command.Request.DocumentIds),
            cancellationToken);
        await repository.EnqueueJobAsync(jobId);
        return new GenerateQuizCommandResponse(jobId);
    }
}