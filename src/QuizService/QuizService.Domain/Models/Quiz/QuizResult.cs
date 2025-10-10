using QuizService.Domain.Abstraction;
using QuizService.Domain.Models.Quiz;

namespace QuizService.Domain.ValuesObject;

public class QuizResult:Entity<QuizResultId>
{
    public QuizId QuizId { get;}
    public Guid UserId { get;}
    public DateTime StartedAt { get;}
    public DateTime? SubmittedAt { get;}
    public int TotalQuestions { get;}
    public int CorrectCount { get;}
    public int Score { get;}
    public List<Guid> QuestionIds { get; } = new();
    public string? SummaryText { get;}
    public string? SummaryModel { get;}
    public DateTime? SummaryCreatedAt { get;}

    protected QuizResult()
    {
        
    }
    private QuizResult(QuizResultId resultId,
        Guid userId,
        QuizId quizId,
        DateTime startedAt,
        DateTime? submittedAt,
        int totalQuestions,
        int correctCount,
        List<Guid> questionIds,
        string? summaryText = null,
        string? summaryModel = null,
        DateTime? summaryCreatedAt = null)
    {
        Id = resultId;
        QuizId = quizId;
        UserId = userId;
        StartedAt = startedAt;
        SubmittedAt = submittedAt;
        TotalQuestions = totalQuestions;
        CorrectCount = correctCount;
        Score = Score = totalQuestions > 0 ? (int)Math.Round(correctCount / (double)totalQuestions * 100) : 0;
        QuestionIds = questionIds;
        SummaryText = summaryText;
        SummaryModel = summaryModel;
        SummaryCreatedAt = summaryCreatedAt;
    }
    public static QuizResult Of(
        Guid userId,
        QuizId quizId,
        DateTime startedAt,
        DateTime? submittedAt,
        int totalQuestions,
        int correctCount,
        List<Guid> questionIds ,
        string? summaryText = null,
        string? summaryModel = null,
        DateTime? summaryCreatedAt = null)
    {
        var newId = QuizResultId.Of(Guid.NewGuid());
        return new QuizResult(newId, userId, quizId, startedAt, submittedAt, totalQuestions, correctCount, questionIds, summaryText, summaryModel, summaryCreatedAt);
    }
}