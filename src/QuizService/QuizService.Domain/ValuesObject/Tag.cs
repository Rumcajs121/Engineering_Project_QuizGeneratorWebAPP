using QuizService.Domain.Abstraction;
using QuizService.Domain.Models.Quiz;

namespace QuizService.Domain.ValuesObject;

public class Tag:Entity<QuizTagId>
{
    private const int DefaultLength = 3;
    public string? Name { get;}

    public Tag()
    {
        //FOR EF
    }
    private Tag(string value)=>Name=value;
    
    public static Tag Of(string value)
         {
             ArgumentNullException.ThrowIfNullOrWhiteSpace(value);
             ArgumentOutOfRangeException.ThrowIfNotEqual(value.Length,DefaultLength);
             return new Tag(value);
         }
}