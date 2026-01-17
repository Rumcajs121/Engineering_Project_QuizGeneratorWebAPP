
using QuizService.Domain.Abstraction;
using QuizService.Domain.Enums;
using QuizService.Domain.Exceptions;
using QuizService.Domain.Models.Quiz.Snapshots;
using QuizService.Domain.ValuesObject;

namespace QuizService.Domain.Models.Quiz;

public class Quiz:Aggregate<QuizId>
{
    public QuizStatus QuizStatus { get; private set; }
    public Guid ExternalId { get; private set; }
    public List<Guid> SourceId { get; private set; }
    private readonly List<QuizQuestion> _questions = new();
    public IReadOnlyCollection<QuizQuestion> Questions => _questions.AsReadOnly();
    
    private readonly List<Tag> _tags = new();
    public IReadOnlyCollection<Tag> Tags => _tags.AsReadOnly();
    public string? Title { get; private set; }
    
    
    public static Quiz Create(List<Guid> sourceId, Guid externalId,string? titleQuiz, IEnumerable<QuizQuestion> questions, IEnumerable<Tag>? tags = null)
    {
        if (sourceId is null ||sourceId.Count == 0  )
            throw new DomainException("SourceId is required.");
        if (titleQuiz != null)
        {
            titleQuiz = titleQuiz.Trim();
            if (titleQuiz.Length == 0) titleQuiz = null;
            if (titleQuiz?.Length > 1000) 
                throw new DomainException("Title must be ≤ 1000 chars.");
        }
        var quiz = new Quiz
        {
            Id = QuizId.Of(Guid.NewGuid()),
            QuizStatus = QuizStatus.Generating,
            SourceId = sourceId,
            ExternalId= externalId,
            Title = titleQuiz
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
    public QuizSnapshot QuizToSnapshot()
        => new QuizSnapshot(
            QuizId: QuizId.Of(Id.Value), 
            QuizName:Title,
            Status: this.QuizStatus.ToString(),
            ExternalId:this.ExternalId,
            SourceId: this.SourceId,
            ShortDescription: this.Title,
            Questions: this.Questions
                .Select(q => new QuestionSnapshot(
                    q.Id.Value,
                    q.Text,
                    q.Explanation,
                    q.SourceChunkId,
                    q.Answers
                        .OrderBy(a => a.Ordinal)
                        .Select(a => new AnswerSnapshot(a.Id.Value, a.Ordinal, a.Text, a.IsCorrect))
                        .ToArray()
                ))
                .ToArray(),
            Tags: this.Tags.Select(t => t.Name!).ToArray()
        );

    protected Quiz() { }
}