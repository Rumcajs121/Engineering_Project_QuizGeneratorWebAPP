using QuizService.Application.Dtos;
using QuizService.Domain;

namespace QuizService.Application.UseCases;

public interface IQuizAttemptService
{
    Task<SubmitQuizAnswerResultDto> SubmitQuiz(SubmitQuizAnswersDto submitDto);
    Task<QuizAttempt> CreateNewAnswer(Guid orderId);
    
}