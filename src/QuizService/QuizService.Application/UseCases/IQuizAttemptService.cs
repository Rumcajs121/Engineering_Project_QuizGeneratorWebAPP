using QuizService.Application.Dtos;

namespace QuizService.Application.UseCases;

public interface IQuizAttemptService
{
    Task<int> SubmitQuiz(SubmitQuizAnswersDto submitDto);
}