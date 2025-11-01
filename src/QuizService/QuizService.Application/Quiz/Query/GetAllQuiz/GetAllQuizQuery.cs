using BuildingBlocks.CQRS;
using QuizService.Application.Dtos;

namespace QuizService.Application.Quiz.Query.GetAllQuiz;

public  record GetAllQuizResult(List<ShortQuizDto> QuizzesShortInfo);
public record GetAllQuizQuery:IQuery<GetAllQuizResult>;