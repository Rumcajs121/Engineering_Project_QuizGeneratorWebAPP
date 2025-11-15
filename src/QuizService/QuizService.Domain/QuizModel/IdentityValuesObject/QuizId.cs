
using Newtonsoft.Json;
using QuizService.Domain.Exceptions;

namespace QuizService.Domain.ValuesObject;

public record QuizId
{
    public Guid Value { get;}

    [JsonConstructor]
    public QuizId(Guid value)
    {
        if (value == Guid.Empty)
        {
            throw new DomainException("Quiz Id cannot be empty");
        }

        Value = value;
    }


    public static QuizId Of(Guid value) => new(value);
    
    protected QuizId()
    {
        
    }
}