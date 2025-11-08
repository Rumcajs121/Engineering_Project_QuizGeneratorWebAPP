using BuildingBlocks.CQRS;

namespace QuizService.Application.Quiz.Command.SubmitQuizCommand;

public class SubmitQuizCommandHandler:ICommandHandler<SubmitQuizCommand,SubmitQuizCommandResult>
{
    public Task<SubmitQuizCommandResult> Handle(SubmitQuizCommand request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}