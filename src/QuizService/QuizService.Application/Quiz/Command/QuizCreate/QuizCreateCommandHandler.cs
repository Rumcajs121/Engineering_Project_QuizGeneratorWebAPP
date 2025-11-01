using BuildingBlocks.CQRS;
using QuizService.Application.Dtos;
using QuizService.Domain.Abstraction;
using QuizService.Domain.Abstraction.Repository;
using QuizService.Domain.Models.Quiz;
using QuizService.Domain.ValuesObject;

namespace QuizService.Application.Quiz.Command.QuizCreate;

public class QuizCreateCommandHandler(IUnitOfWork unitOfWork, IQuizRepository quizRepository,ITagRepository tagRepository)
    : ICommandHandler<QuizCreateCommand, CreateQuizResult>
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly IQuizRepository _quizRepository = quizRepository;
    private readonly ITagRepository _tagRepository = tagRepository;

    public async Task<CreateQuizResult> Handle(QuizCreateCommand command, CancellationToken cancellationToken)
    {
        var result=await CreateNewQuiz(command.CreateQuizDto);
        await _quizRepository.AddAsync(result, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        
        return new CreateQuizResult(result.Id.Value);
    }

    private async Task<Domain.Models.Quiz.Quiz> CreateNewQuiz(CreateQuizDto dto)
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

        var tag=await CreateTag(dto.Tag);
        
        var newQuiz = Domain.Models.Quiz.Quiz.Create(
            sourceId: dto.SourceId,
            shortDescription: dto.ShortDescription,
            questions: listNewQuestion,
            tags: tag
        );
        
        return newQuiz;
    }

    private async Task<List<Tag>> CreateTag(List<string> tagName)
    {
        List<Tag> allTags = new();
        var existing = await _tagRepository.GetByNameAsync(tagName);
        var existingNames = existing
            .Select(t => t.Name)
            .ToHashSet(StringComparer.OrdinalIgnoreCase);
        var missing = tagName
            .Where(n => !existingNames.Contains(n))
            .Select(Tag.Of)
            .ToList();
        if (missing.Count > 0)
        {
            _tagRepository.AddRange(missing);
        }
        allTags = existing.Concat(missing).ToList();
        return allTags;
    }
}