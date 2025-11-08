using BuildingBlocks.CQRS;
using QuizService.Application.Dtos;
using QuizService.Application.Mappers;
using QuizService.Domain.Abstraction;
using QuizService.Domain.IdentityValuesObject;

namespace QuizService.Application.Quiz.Query.GetAttemptQuizById;

public class GetAttemptQuizQueryHandler(IQuizAttemptRepository repository):IQueryHandler<GetAttemptQuizByIdQuery,GetAttemptQuizByIdResult>
{
    private readonly IQuizAttemptRepository _repository = repository;

    public async Task<GetAttemptQuizByIdResult> Handle(GetAttemptQuizByIdQuery query, CancellationToken cancellationToken)
    {
        var quizAttempt= await _repository.GetAttemptQuizByIdAsync(QuizAttemptId.Of(query.QuizAttemptId));
        var attemptDto = QuizAttemptMapping.QuizAttemptToDto(quizAttempt);
        return new GetAttemptQuizByIdResult(attemptDto);
    }
}