using QuizService.Domain.Abstraction;
using QuizService.Domain.ValuesObject;

namespace QuizService.Domain.Models.Quiz;

public class QuizQuestion : Entity<QuizQuestionId>
{
    public string Text { get; }
    public string? Explanation { get; }
    public Guid? SourceChunkId { get; }
    
    private readonly List<QuizAnswer> _answers = new();
    public IReadOnlyCollection<QuizAnswer> Answers => _answers.AsReadOnly();
    protected QuizQuestion()
    {
        
    }
}
