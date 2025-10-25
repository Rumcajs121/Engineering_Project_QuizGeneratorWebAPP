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
    
    public static QuizAttemptQuestion Of(QuizAttemptQuestionId quizAttemptQuestionId,QuizQuestionId quizQuestionId, 
        List<Guid> selectedAnswerIds,List<Guid> correctAnswerIds)
    {
        var qa = new QuizAttemptQuestion
        { 
            Id = quizAttemptQuestionId,
            QuizQuestionId = quizQuestionId
        };
        qa._correctAnswerIds.AddRange(correctAnswerIds);
        if (selectedAnswerIds == null || selectedAnswerIds.Count == 0)
            throw new DomainException("You have to select an answer.");
        qa._selectedAnswerIds.AddRange(selectedAnswerIds);
        qa.IsCorrect = qa.CheckAnswer();
        return qa;
    }
    public bool CheckAnswer()
    {
        return _selectedAnswerIds
            .OrderBy(id=>id)
            .SequenceEqual(_correctAnswerIds
                .OrderBy(id=>id));
        
    }

    protected QuizAttemptQuestion()
    {
        
    }
}