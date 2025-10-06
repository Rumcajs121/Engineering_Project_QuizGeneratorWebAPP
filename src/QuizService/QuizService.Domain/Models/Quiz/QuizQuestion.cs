using QuizService.Domain.Abstraction;
using QuizService.Domain.ValuesObject;

namespace QuizService.Domain.Models.Quiz;

public class QuizQuestion:Entity<QuizQuestionId>
{
    public Guid QuizId{ get; set; }
    public Quiz Quiz{ get; set; }
    public string Text{ get; set; }
    public string? Explanation{ get; set; }
    public Guid? SourceChunkId{ get; set; }
    private readonly List<QuizAnswer> _answers = new();
    public IReadOnlyCollection<QuizAnswer> Answers => _answers.AsReadOnly();
    private readonly List<QuizTag> _tags = new();
    public IReadOnlyCollection<QuizTag> Tags => _tags.AsReadOnly();
}