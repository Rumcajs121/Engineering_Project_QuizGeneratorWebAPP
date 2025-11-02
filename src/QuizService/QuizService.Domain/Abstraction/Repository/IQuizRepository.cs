using QuizService.Domain.Models.Quiz;
using QuizService.Domain.ValuesObject;

namespace QuizService.Domain.Abstraction;

public interface IQuizRepository:IRepository<Quiz,QuizId>
{
    Task<List<Quiz>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<Quiz>GetByIdAsync(QuizId id, CancellationToken cancellationToken = default);
    
    
}