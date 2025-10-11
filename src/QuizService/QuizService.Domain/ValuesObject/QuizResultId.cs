using QuizService.Domain.Exceptions;

namespace QuizService.Domain.ValuesObject;

public record QuizResultId
{
    public Guid Value { get;}

    private QuizResultId(Guid value)=>Value = value;

    public static QuizResultId Of(Guid value)
    {
        ArgumentNullException.ThrowIfNull(value);
        if (value == Guid.Empty)
        {
            throw new DomainException("QuizResultId cannot be empty");
        }
        return new QuizResultId(value);
    }
}