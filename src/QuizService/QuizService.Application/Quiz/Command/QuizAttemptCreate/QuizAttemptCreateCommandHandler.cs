using BuildingBlocks.CQRS;
using QuizService.Application.Mappers;
using QuizService.Application.UseCases;
using QuizService.Domain.Abstraction;

namespace QuizService.Application.Quiz.Command.QuizAttemptCreate;

public class QuizAttemptCreateCommandHandler(IQuizAttemptService service, IUnitOfWork uow,IQuizAttemptRepository repository):ICommandHandler<QuizAttemptCreateCommand,QuizAttemptCreateCommandResult>
{
    public async Task<QuizAttemptCreateCommandResult> Handle(QuizAttemptCreateCommand request, CancellationToken cancellationToken)
    {
        var createNewQuizAttempt=await service.CreateNewAnswer(request.QuizId);
        await repository.AddAsync(createNewQuizAttempt,cancellationToken);
        await uow.SaveChangesAsync(cancellationToken);
        
        return new QuizAttemptCreateCommandResult(QuizAttemptMapping.QuizAttemptToDto(createNewQuizAttempt));
    }
}