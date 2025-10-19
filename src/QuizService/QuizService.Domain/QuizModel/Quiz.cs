
using QuizService.Domain.Abstraction;
using QuizService.Domain.Enums;
using QuizService.Domain.Exceptions;
using QuizService.Domain.ValuesObject;

namespace QuizService.Domain.Models.Quiz;

public class Quiz:Aggregate<QuizId>
{
    public QuizStatus QuizStatus { get; private set; }
    public Guid SourceId { get; private set; }
    private readonly List<QuizQuestion> _questions = new();
    public IReadOnlyCollection<QuizQuestion> Questions => _questions.AsReadOnly();
    
    private readonly List<Tag> _tags = new();
    public IReadOnlyCollection<Tag> Tags => _tags.AsReadOnly();
    public string? ShortDescription { get; private set; }
    
    
    public static Quiz Create(Guid sourceId, string? shortDescription, IEnumerable<QuizQuestion> questions, IEnumerable<Tag>? tags = null)
    {
        if (sourceId == Guid.Empty)
            throw new DomainException("SourceId is required.");
        if (shortDescription != null)
        {
            shortDescription = shortDescription.Trim();
            if (shortDescription.Length == 0) shortDescription = null;
            if (shortDescription?.Length > 500) 
                throw new DomainException("ShortDescription must be ≤ 500 chars.");
        }
        var quiz = new Quiz
        {
            Id = QuizId.Of(Guid.NewGuid()),
            QuizStatus = QuizStatus.Generating,
            SourceId = sourceId,
            ShortDescription = shortDescription
        };
        foreach (var q in questions ?? Enumerable.Empty<QuizQuestion>())
            quiz.AddQuestion(q);

        if (!quiz._questions.Any())
            throw new DomainException("Quiz must contain at least one question.");

        if (tags != null)
            foreach (var t in tags)
                quiz.AddTag(t);

        return quiz;
    }

    public  void AddTag(Tag tag)
    {
        if (tag == null) throw new ArgumentNullException(nameof(tag));
        if (_tags.Any(t => t.Id == tag.Id)) return;
        _tags.Add(tag);
    }

    public  void AddQuestion(QuizQuestion question)
    {
        if (question == null) throw new ArgumentNullException(nameof(question));
        if (_questions.Any(q => q.Id == question.Id))
            throw new DomainException("Question already added to this quiz.");
        _questions.Add(question);
    }

    protected Quiz() { }
}