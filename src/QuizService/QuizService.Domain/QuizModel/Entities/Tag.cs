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
        //FOR EF
    }
    private Tag(string value)=>Name=value;
    
    public static Tag Of(string value)
         {
             ArgumentNullException.ThrowIfNullOrWhiteSpace(value);
             return new Tag(value);
         }
}