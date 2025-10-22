using QuizService.Domain.Abstraction;
using QuizService.Domain.Exceptions;
using QuizService.Domain.IdentityValuesObject;
using QuizService.Domain.Models.Quiz;
using QuizService.Domain.ValuesObject;

namespace QuizService.Domain.Entities;

public class QuizAttemptQuestion:Entity<QuizAttemptQuestionId>
{
    public QuizQuestionId QuizQuestionId { get; private set; }
    private readonly List<QuizAnswerId> _selectedAnswerIds = new();
    private readonly List<QuizAnswerId> _correctAnswerIds = new();
    public bool IsCorrect { get; private set; }
    
    public static QuizAttemptQuestion Of(QuizAttemptQuestionId quizAttemptQuestionId , 
        QuizQuestionId quizQuestionId,List<QuizAnswerId> selectedAnswerIds,List<QuizAnswerId> correctAnswerIds,bool isCorrect )
    {
        var qa = new QuizAttemptQuestion();
        qa.Id = QuizAttemptQuestionId.Of(Guid.NewGuid());
        
        qa._correctAnswerIds.AddRange(correctAnswerIds);
        if (selectedAnswerIds == null || selectedAnswerIds.Count == 0)
            throw new DomainException("You have to select an answer.");
        qa._selectedAnswerIds.AddRange(selectedAnswerIds);
        qa.IsCorrect = qa.CheckAnswear();
        return qa;
    }
    public bool CheckAnswear()
    {
        return _selectedAnswerIds
            .OrderBy(id=>id.Value)
            .SequenceEqual(_correctAnswerIds
                .OrderBy(id=>id.Value));
        
    }

    protected QuizAttemptQuestion()
    {
        
    }
}