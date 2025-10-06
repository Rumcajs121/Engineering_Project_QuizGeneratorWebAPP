using QuizService.Domain.Exceptions;

namespace QuizService.Domain.ValuesObject;

public class QuizId
{
    public Guid Value { get;}

    private QuizId(Guid value)=>Value = value;

    public static QuizId Of(Guid value)
    {
        ArgumentNullException.ThrowIfNull(value);
        if (value == Guid.Empty)
        {
            throw new DomainException("Quiz Id cannot be empty");
        }
        return new QuizId(value);
    }
}