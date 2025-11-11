using QuizService.Application.Dtos;
using QuizService.Domain.ValuesObject;

namespace QuizService.Application.UseCases;

public interface IQuizService
{
    Task<Domain.Models.Quiz.Quiz> CreateNewQuiz(CreateQuizDto dto);
}