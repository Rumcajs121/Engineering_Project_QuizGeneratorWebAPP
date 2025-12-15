using BuildingBlocks.CQRS;
using LLMService.Commons.Models;

namespace LLMService.Features.GenerateQuiz;
public record GenerateQuizCommandResponse(LlmQuiz Quiz);
public record GenerateQuizCommandRequest(int K, int CountQuestion,string Question, IReadOnlyList<Guid> DocumentIds);
public record GenerateQuizCommand(GenerateQuizCommandRequest Request) : ICommand<GenerateQuizCommandResponse>;
public class GenerateQuizCommandHandler(IGenerateQuizService service):ICommandHandler<GenerateQuizCommand, GenerateQuizCommandResponse>
{
    public async Task<GenerateQuizCommandResponse> Handle(GenerateQuizCommand command, CancellationToken cancellationToken)
    {
        var result=await service.GenerateQuiz(command.Request.K,command.Request.CountQuestion,command.Request.Question,command.Request.DocumentIds);
        return new GenerateQuizCommandResponse(result);
    }
}