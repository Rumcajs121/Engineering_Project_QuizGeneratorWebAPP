using QuizService.Domain.Exceptions;

namespace QuizService.Domain.ValuesObject;

public record QuizQuestionId
{
    public Guid Value { get;}

    private QuizQuestionId(Guid value)=>Value = value;

    public static QuizQuestionId Of(Guid value)
    {
        ArgumentNullException.ThrowIfNull(value);
        if (value == Guid.Empty)
        {
            throw new DomainException("QuizQuestionId cannot be empty");
        }
        return new QuizQuestionId(value);
    }
}