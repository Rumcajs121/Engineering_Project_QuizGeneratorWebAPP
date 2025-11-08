using QuizService.Domain.IdentityValuesObject;
using QuizService.Domain.Models.Quiz;
using QuizService.Domain.ValuesObject;

namespace QuizService.Domain.Abstraction;

public interface IQuizAttemptRepository:IRepository<QuizAttempt,QuizAttemptId>
{
    Task<QuizAttempt>GetAttemptQuizByIdAsync(QuizAttemptId id, CancellationToken cancellationToken = default);
    
}