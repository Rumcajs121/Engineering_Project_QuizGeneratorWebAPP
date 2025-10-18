using QuizService.Domain.Exceptions;

namespace QuizService.Domain.ValuesObject;

public record QuizTagId
{
    protected QuizTagId()
    {
        
    }
    public Guid Value { get;}

    private QuizTagId(Guid value)=>Value = value;

    public static QuizTagId Of(Guid value)
    {
        ArgumentNullException.ThrowIfNull(value);
        if (value == Guid.Empty)
        {
            throw new DomainException("QuizTagIdcannot be empty");
        }
        return new QuizTagId(value);
    }
}