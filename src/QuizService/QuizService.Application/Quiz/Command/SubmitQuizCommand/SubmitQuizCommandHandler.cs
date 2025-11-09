using BuildingBlocks.CQRS;
using QuizService.Application.UseCases;
using QuizService.Domain.Abstraction;

namespace QuizService.Application.Quiz.Command.SubmitQuizCommand;

public class SubmitQuizCommandHandler(IQuizAttemptService _service, IUnitOfWork _uow):ICommandHandler<SubmitQuizCommand,SubmitQuizCommandResult>
{
    public async Task<SubmitQuizCommandResult> Handle(SubmitQuizCommand request, CancellationToken cancellationToken)
    {
        var submitQuiz =await  _service.SubmitQuiz(request.dto);
        await _uow.SaveChangesAsync(cancellationToken);
        return new SubmitQuizCommandResult(submitQuiz);
    }
}
