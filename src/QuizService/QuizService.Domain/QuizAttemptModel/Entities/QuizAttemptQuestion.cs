using QuizService.Domain.Abstraction;
using QuizService.Domain.Exceptions;
using QuizService.Domain.IdentityValuesObject;
using QuizService.Domain.Models.Quiz;
using QuizService.Domain.ValuesObject;

namespace QuizService.Domain.Entities;

public class QuizAttemptQuestion:Entity<QuizAttemptQuestionId>
{
    public QuizQuestionId QuizQuestionId { get; private set; }
    public IReadOnlyList<Guid> SelectedAnswerIds => _selectedAnswerIds.AsReadOnly();
    public IReadOnlyList<Guid> CorrectAnswerIds => _correctAnswerIds.AsReadOnly();
    private readonly List<Guid> _selectedAnswerIds = new();
    private readonly List<Guid> _correctAnswerIds = new();
    public bool IsCorrect { get; private set; }
    
    public static QuizAttemptQuestion Of(QuizAttemptQuestionId quizAttemptQuestionId,QuizQuestionId quizQuestionId 
   ,List<Guid> correctAnswerIds)
    {
        var qa = new QuizAttemptQuestion
        { 
            Id = quizAttemptQuestionId,
            QuizQuestionId = quizQuestionId
        };
        qa._correctAnswerIds.AddRange(correctAnswerIds);
        return qa;
    }
    public void ApplySelection(IEnumerable<Guid> selectedIds)
    {
        _selectedAnswerIds.Clear();
        _selectedAnswerIds.AddRange((selectedIds ?? Enumerable.Empty<Guid>()).Distinct());
        IsCorrect = CheckAnswer();
    }


    public bool CheckAnswer()
    {
        var selected = _selectedAnswerIds.ToHashSet();
        var correct  = _correctAnswerIds.ToHashSet();
        return selected.SetEquals(correct);
    }

    protected QuizAttemptQuestion()
    {
        
    }
}