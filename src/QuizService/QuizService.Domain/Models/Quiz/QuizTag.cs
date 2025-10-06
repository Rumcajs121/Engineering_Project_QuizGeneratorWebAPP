using QuizService.Domain.Abstraction;
using QuizService.Domain.ValuesObject;

namespace QuizService.Domain.Models.Quiz;

public class QuizTag:Entity<QuizTagId>
{
    // TODO: encja n:n ?? 
    public Guid QuestionId { get; set; }
    public string? Name { get; set; }
}