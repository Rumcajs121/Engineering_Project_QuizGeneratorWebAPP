using QuizService.Domain.Exceptions;

namespace QuizService.Domain.IdentityValuesObject;

public record QuizAttemptId
{
    public Guid Value { get;}

    private QuizAttemptId(Guid value)=>Value = value;

    protected QuizAttemptId()
    {
        
    }

    public static QuizAttemptId Of(Guid value)
    {
        ArgumentNullException.ThrowIfNull(value);
        if (value == Guid.Empty)
        {
            throw new DomainException("QuizAttempt Id cannot be empty");
        }

        return new QuizAttemptId(value);
    }
    
    
}