using QuizService.Domain.Exceptions;

namespace QuizService.Domain.ValuesObject;

public record QuizAnswerId
{
    protected QuizAnswerId()
    {
        
    }
    public Guid Value { get;}

    private QuizAnswerId(Guid value)=>Value = value;

    public static QuizAnswerId Of(Guid value)
    {
        ArgumentNullException.ThrowIfNull(value);
        if (value == Guid.Empty)
        {
            throw new DomainException("QuizAnswerId cannot be empty");
        }
        return new QuizAnswerId(value);
    }
}