using QuizService.Domain.Abstraction;
using QuizService.Domain.ValuesObject;

namespace QuizService.Domain.Models.Quiz;

public class QuizAnswer:Entity<QuizAnswerId>
{
    public QuizQuestionId QuizQuestionId {get; }
    public int Ordinal {get;}
    public string Text {get;}
    public bool IsCorrect {get;}
    

    protected QuizAnswer()
    {
        //For EF
    }

    private QuizAnswer(QuizAnswerId id, QuizQuestionId quizQuestionId,bool isCorrect, string text, int ordinal)
    {
        Id = id;
        QuizQuestionId = quizQuestionId;
        Ordinal = ordinal;
        Text = text;
        IsCorrect = isCorrect;
    }

    public static QuizAnswer Of(QuizAnswerId id,QuizQuestionId quizQuestionId,int ordinal, string text, bool isCorrect)
    {
        ArgumentNullException.ThrowIfNullOrWhiteSpace(text);

        if (ordinal < 0)
            throw new ArgumentOutOfRangeException(nameof(ordinal), "Ordinal must be non-negative.");

        ArgumentNullException.ThrowIfNull(quizQuestionId);

        if (quizQuestionId.Value == Guid.Empty)
            throw new ArgumentException("QuizQuestion must have a valid ID.", nameof(quizQuestionId));
        
        return new QuizAnswer(id,quizQuestionId,isCorrect , text, ordinal);
    }
}