using LLMService.Features.GenerateQuiz;

namespace LLMService.Commons.Models;

public enum QuizJobStatus
{
    Pending,
    Running,
    Generated,
    Sent,
    Failed
}
public class QuizJob
{
    public string JobId { get; set; } = default!;
    public QuizJobStatus Status { get; set; } = QuizJobStatus. Pending;
    public GenerateQuizParameter Parameter { get; set; } = default!;
    public Guid ExternalId { get; set; }
    public LlmQuiz?  Result { get; set; }
    public string? Error { get; set; } 
    public string? QuizServiceId { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime?  CompletedAt { get; set; }
}