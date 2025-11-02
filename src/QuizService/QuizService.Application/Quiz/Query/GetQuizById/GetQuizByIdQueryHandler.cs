using BuildingBlocks.CQRS;
using QuizService.Application.Mappers;
using QuizService.Domain.Abstraction;
using QuizService.Domain.ValuesObject;

namespace QuizService.Application.Quiz.Query.GetQuizById;

public class GetQuizByIdQueryHandler(IQuizRepository repository):IQueryHandler<GetQuizByIdQuery,GetQuizByIdResult>
{
    private readonly IQuizRepository _repository = repository;

    public async Task<GetQuizByIdResult> Handle(GetQuizByIdQuery query, CancellationToken cancellationToken)
    {
        var quiz=await _repository.GetByIdAsync(QuizId.Of(query.QuizId), cancellationToken);
        var quizDto = QuizMapping.ToQuizDto(quiz);
        return new GetQuizByIdResult(quizDto);
    }
}