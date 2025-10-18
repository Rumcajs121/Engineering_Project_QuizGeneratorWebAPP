using QuizService.Domain.Abstraction;
using QuizService.Domain.ValuesObject;

namespace QuizService.Domain.Models.Quiz;

public class QuizAnswer:Entity<QuizAnswerId>
{
    public int Ordinal {get;}
    public string Text {get;}
    public bool IsCorrect {get;}
    protected QuizAnswer()
    {
        //For EF
    }
}