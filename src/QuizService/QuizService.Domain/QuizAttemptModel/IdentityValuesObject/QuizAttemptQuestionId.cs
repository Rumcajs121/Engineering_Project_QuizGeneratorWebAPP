using QuizService.Domain.Exceptions;

namespace QuizService.Domain.IdentityValuesObject;

public record QuizAttemptQuestionId
{
    public Guid Value { get;}

    private QuizAttemptQuestionId(Guid value)=>Value = value;

    protected QuizAttemptQuestionId()
    {
        
    }

    public static QuizAttemptQuestionId Of(Guid value)
    {
        ArgumentNullException.ThrowIfNull(value);
        if (value == Guid.Empty)
        {
            throw new DomainException("QuizAttempt Id cannot be empty");
        }

        return new QuizAttemptQuestionId(value);
    }
}

