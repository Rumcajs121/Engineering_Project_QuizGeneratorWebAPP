using BuildingBlocks.CQRS;
using QuizService.Application.Dtos;

namespace QuizService.Application.Quiz.Query.GetQuizById;

public record GetQuizByIdResult(QuizDto QuizDto);
public record GetQuizByIdQuery(Guid QuizId):IQuery<GetQuizByIdResult>;