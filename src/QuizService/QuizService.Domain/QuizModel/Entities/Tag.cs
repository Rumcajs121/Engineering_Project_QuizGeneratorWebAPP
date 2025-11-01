using QuizService.Domain.Abstraction;
using QuizService.Domain.Models.Quiz;

namespace QuizService.Domain.ValuesObject;

public class Tag:Entity<QuizTagId>
{
    public string? Name { get; private set; }
    
    private readonly List<Quiz> _quizzes = new();
    public IReadOnlyCollection<Quiz> Quizzes => _quizzes .AsReadOnly();
    
    protected Tag()
    {
        //FOR EFc
    }
    
    private Tag(string value)=>Name=value;
    
    public static Tag Of(string value)
    {
        ArgumentNullException.ThrowIfNullOrWhiteSpace(value);
        var normalized = value.Trim();
        return new Tag(normalized)
        {
            Id = QuizTagId.Of(Guid.NewGuid())
        };
    }
}