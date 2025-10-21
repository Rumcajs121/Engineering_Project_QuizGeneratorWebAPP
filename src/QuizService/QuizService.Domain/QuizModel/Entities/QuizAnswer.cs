using QuizService.Domain.Abstraction;
using QuizService.Domain.Exceptions;
using QuizService.Domain.ValuesObject;

namespace QuizService.Domain.Models.Quiz;

public class QuizAnswer:Entity<QuizAnswerId>
{
    internal QuizAnswer( int ordinal,string text,bool isCorrect)
    {
        Id = QuizAnswerId.Of(Guid.NewGuid());
        UpdateAnswers(ordinal, text,isCorrect);
        
    }
    public int Ordinal {get;private set;}
    public string Text {get;private set;}
    public bool IsCorrect {get;private set;}

    internal void UpdateAnswers(int ordinal,string text,bool isCorrect)
    {
        ValidateArgument(ordinal, text);
        Ordinal=ordinal;
        Text=text.Trim();
        IsCorrect=isCorrect;
    }
    private void ValidateArgument(int ordinal, string text)
    {
        if (ordinal < 0) throw new DomainException("Ordinal must be >= 0");
        if(text.Length>1000 || string.IsNullOrWhiteSpace(text)) throw new DomainException("Answer text cannot be blank");
    }

    protected QuizAnswer() { }
    
}