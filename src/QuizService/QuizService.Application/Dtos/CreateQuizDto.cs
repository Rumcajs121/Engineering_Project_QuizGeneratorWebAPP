namespace QuizService.Application.Dtos;

public class CreateQuizDto
{
    public Guid QuizId { get; init; }
    public string QuizStatus { get; init; }
    public Guid SourceId { get; init; }
    public IEnumerable<CreateQusestionDto> Question { get; init; }
    public IEnumerable<string> Tag { get; init; }
    public string Title { get; init; }
    public DateTimeOffset CreatedAt { get; init; }
}

public class CreateQusestionDto
{
    public Guid QuestionId { get; init; }
    public string Text { get; init; }
    public string Explanation { get; init; }
    public Guid SourceChunkId { get; init; }
    public IEnumerable<CreateAnswerDto> Answer { get; init; }
}

public class CreateAnswerDto
{
    public Guid AnswerId { get; init; }
    public int Ordinal { get; init; }
    public string Text { get; init; }
    public bool IsCorrect { get; init; }
}