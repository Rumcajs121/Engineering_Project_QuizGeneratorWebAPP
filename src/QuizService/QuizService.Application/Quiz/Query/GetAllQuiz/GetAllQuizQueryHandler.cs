using BuildingBlocks.CQRS;
using QuizService.Application.Mappers;
using QuizService.Domain.Abstraction;

namespace QuizService.Application.Quiz.Query.GetAllQuiz;


public class GetAllQuizQueryHandler(IQuizRepository repository):IQueryHandler<GetAllQuizQuery, GetAllQuizResult>
{
    private readonly IQuizRepository _repository = repository;

    public async Task<GetAllQuizResult> Handle(GetAllQuizQuery query, CancellationToken cancellationToken)
    {
       var orginalQuiz= await _repository.GetAllAsync(cancellationToken);
       var result=QuizMapping.ToShortQuizDto(orginalQuiz);
       return new GetAllQuizResult(result);
    }
}