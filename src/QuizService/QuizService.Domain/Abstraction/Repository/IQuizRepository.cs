using QuizService.Domain.Models.Quiz;
using QuizService.Domain.ValuesObject;

namespace QuizService.Domain.Abstraction;

public interface IQuizRepository:IRepository<Quiz,QuizId>
{
    
}