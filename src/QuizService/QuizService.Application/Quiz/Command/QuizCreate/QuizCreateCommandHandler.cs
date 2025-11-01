using BuildingBlocks.CQRS;
using QuizService.Application.Dtos;
using QuizService.Domain.Abstraction;
using QuizService.Domain.Models.Quiz;

namespace QuizService.Application.Quiz.Command.QuizCreate;

public class QuizCreateCommandHandler(IUnitOfWork unitOfWork, IQuizRepository quizRepository)
    : ICommandHandler<QuizCreateCommand, CreateQuizResult>
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly IQuizRepository _quizRepository = quizRepository;

    public async Task<CreateQuizResult> Handle(QuizCreateCommand command, CancellationToken cancellationToken)
    {
        var result=CreateNewQuiz(command.CreateQuizDto);
        await _quizRepository.AddAsync(result, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        
        return new CreateQuizResult(result.Id.Value);
    }

    private Domain.Models.Quiz.Quiz CreateNewQuiz(CreateQuizDto dto)
    {
        //TODO: CRETAE FABRIC INTERFEJS ? 
        var listNewQuestion=new List<QuizQuestion>();
        foreach (var qDto in dto.Question)
        {
            var answersTuples = qDto.Answer.Select(a => (a.Ordinal, a.Text, a.IsCorrect));
            var newQuestion = QuizQuestion.Of(
                text: qDto.Text,
                explanation: qDto.Explanation,
                sourceChunkId: qDto.SourceChunkId,
                initialAnswers: answersTuples
            );
            listNewQuestion.Add(newQuestion);
        }
        var newQuiz = Domain.Models.Quiz.Quiz.Create(
            sourceId: dto.SourceId,
            shortDescription: dto.ShortDescription,
            questions: listNewQuestion
            //TODO;Tags add?? 
        );
        
        return newQuiz;
    }
}