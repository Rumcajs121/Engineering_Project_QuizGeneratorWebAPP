using BuildingBlocks.CQRS;
using QuizService.Application.Dtos;
using QuizService.Application.UseCases;
using QuizService.Domain.Abstraction;
using QuizService.Domain.Abstraction.Repository;
using QuizService.Domain.Models.Quiz;
using QuizService.Domain.ValuesObject;

namespace QuizService.Application.Quiz.Command.QuizCreate;

public class QuizCreateCommandHandler(IUnitOfWork unitOfWork, IQuizRepository quizRepository,ITagRepository tagRepository,IQuizService quizService)
    : ICommandHandler<QuizCreateCommand, CreateQuizResult>
{

    public async Task<CreateQuizResult> Handle(QuizCreateCommand command, CancellationToken cancellationToken)
    {
        var result=await quizService.CreateNewQuiz(command.CreateQuizDto);
        await quizRepository.AddAsync(result, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);
        
        return new CreateQuizResult(result.Id.Value);
    }
}