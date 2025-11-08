using BuildingBlocks.CQRS;
using QuizService.Application.Dtos;

namespace QuizService.Application.Quiz.Query.GetAttemptQuizById;

public record GetAttemptQuizByIdResult(QuizAttemptViewDto AttemptByUser);
public record GetAttemptQuizByIdQuery(Guid QuizAttemptId):IQuery<GetAttemptQuizByIdResult>;
