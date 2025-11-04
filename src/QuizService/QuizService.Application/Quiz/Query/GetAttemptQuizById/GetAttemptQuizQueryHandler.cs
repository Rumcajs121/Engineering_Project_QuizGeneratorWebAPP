using BuildingBlocks.CQRS;
using QuizService.Domain.Abstraction;
using QuizService.Domain.IdentityValuesObject;

namespace QuizService.Application.Quiz.Query.GetAttemptQuizById;

public class GetAttemptQuizQueryHandler(IQuizAttemptRepository repository):IQueryHandler<GetAttemptQuizByIdQuery,GetAttemptQuizByIdResult>
{
    private readonly IQuizAttemptRepository _repository = repository;

    public async Task<GetAttemptQuizByIdResult> Handle(GetAttemptQuizByIdQuery query, CancellationToken cancellationToken)
    {
        var quizAttemptById= await _repository.GetAttemptQuizByIdAsync(QuizAttemptId.Of(query.QuizAttemptId));
        
        throw new NotImplementedException();
    }
}