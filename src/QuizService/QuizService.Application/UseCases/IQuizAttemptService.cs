using QuizService.Application.Dtos;
using QuizService.Domain;

namespace QuizService.Application.UseCases;

public interface IQuizAttemptService
{
    Task<int> SubmitQuiz(SubmitQuizAnswersDto submitDto);
    Task<QuizAttempt> CreateNewAnswer(Guid orderId);
    

}