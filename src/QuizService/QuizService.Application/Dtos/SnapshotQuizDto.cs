namespace QuizService.Application.Dtos;

public class SnapshotQuizDto
{
    public Guid SnapshotId { get; init; }
    public Guid QuizId { get; init; }
    public Guid Id { get; init; }
    public string QuizStatus { get; init; }
    public Guid? SourceId { get; init; }
    public IEnumerable<QusestionSnapshotDto> Question { get; init; }
    public IEnumerable<string> Tag { get; init; }
    public string ShortDescription { get; init; }
    public DateTimeOffset CreatedAt { get; init; }
    public int Version { get; init; }
}

public class QusestionSnapshotDto
{
    public Guid QuestionId { get; init; }
    public string Text { get; init; }
    public string Explanation { get; init; }
    public Guid SourceChunkId { get; init; }
    public IEnumerable<AnswerSnapshotDto> Answer { get; init; }
}

public class AnswerSnapshotDto
{
    public Guid AnswerId { get; init; }
    public int Ordinal { get; init; }
    public string Text { get; init; }
    public bool IsCorrect { get; init; }
}