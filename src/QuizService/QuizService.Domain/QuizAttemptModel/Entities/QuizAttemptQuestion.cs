using QuizService.Domain.Models.Quiz;
using QuizService.Domain.ValuesObject;

namespace QuizService.Domain.Entities;

public class QuizAttemptQuestion
{
    public QuizQuestionId QuizQuestionAttempId { get; set; }
    private readonly List<QuizAnswerId> _selectedAnswerIds = new();
    private readonly List<QuizAnswerId> _correctAnswerIds = new();
    public bool IsCorrect { get; private set; }
}