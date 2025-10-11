namespace QuizService.Domain.ValuesObject;

public record QuizTag
{
    //TODO: Secondary table N:N
    public Guid QuizQuestionId { get; set; }
    public Guid QuizTagId { get; set; }
}