using QuizService.Domain.Abstraction;
using QuizService.Domain.ValuesObject;

namespace QuizService.Domain.Models.Quiz;

public class QuizQuestion : Entity<QuizQuestionId>
{
    public QuizId QuizId { get; }
    public string Text { get; }
    public string? Explanation { get; }
    public Guid? SourceChunkId { get; }
    private readonly List<QuizAnswer> _answers = new();
    public IReadOnlyCollection<QuizAnswer> Answers => _answers.AsReadOnly();
    private readonly List<Tag> _tags = new();
    public IReadOnlyCollection<Tag> Tags => _tags.AsReadOnly();




    protected QuizQuestion()
    {
        
    }

    private QuizQuestion(
        string text,
        string? explanation,
        IEnumerable<(string Text, bool IsCorrect)> answers,IEnumerable<Tag> tags,Guid? sourceChunkId,QuizId quizId)
    {
        ArgumentNullException.ThrowIfNullOrWhiteSpace(text);
             if (answers == null || !answers.Any())
                 throw new ArgumentException("Question must have at least one answer.", nameof(answers));
        
        Id = QuizQuestionId.Of(Guid.NewGuid());
        Text = text;
        Explanation = explanation;

        int ordinal = 0;
        foreach (var (answerText, isCorrect) in answers)
        {
            var answer = QuizAnswer.Of(
                id: QuizAnswerId.Of(Guid.NewGuid()),
                quizQuestionId: this.Id,
                ordinal: ordinal++,
                text: answerText,
                isCorrect: isCorrect
            );
            _answers.Add(answer);
        }
        if (tags != null)
        {
            foreach (var tag in tags)
            {
                AddTag(tag);
            }
                
        }
        SourceChunkId= sourceChunkId;
        if (quizId is null)
            throw new ArgumentNullException(nameof(quizId));
        QuizId = quizId;
    }
    public static QuizQuestion Of(
        string text,
        string? explanation,
        IEnumerable<(string Text, bool IsCorrect)> answers,
        IEnumerable<Tag> tags,
        Guid? sourceChunkId,
        QuizId quizId)
    {
        return new QuizQuestion(text, explanation, answers, tags, sourceChunkId, quizId);
    }

    public void AddTag(Tag tag)
    {
        if (_tags.All(t => t.Name != tag.Name))
            _tags.Add(tag);
    }

    public void RemoveTag(Tag tag)
    {
        _tags.RemoveAll(t=>t.Id == tag.Id);
    }
}
