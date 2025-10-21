using QuizService.Domain.Abstraction;
using QuizService.Domain.Entities;
using QuizService.Domain.IdentityValuesObject;
using QuizService.Domain.ValuesObject;

namespace QuizService.Domain;

public class QuizAttempt:Aggregate<QuizAttemptId>
{
    public QuizId QuizId { get; set; } //reference to quiz
    public string UserId { get; set; } //TODO: reference to user ??
    public string SnapchotQuizJson { get; set; }
    public int Score { get; set; }
    public DateTime StartQuiz { get; set; }
    public DateTime EndTime { get; set; }
    public int  Difficult { get; set; }
    private readonly List<QuizAttemptQuestion> _attemptQuestions= new();
    public IReadOnlyCollection<QuizAttemptQuestion> AttemptQuestions => _attemptQuestions.AsReadOnly();
}