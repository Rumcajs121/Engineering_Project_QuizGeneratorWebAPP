using QuizService.Domain.Enums;
namespace QuizService.Domain.Models.Quiz.Snapshots;

public sealed record QuizSnapshot(
    Guid QuizId,
    string Status,
    Guid SourceId,
    string? ShortDescription,
    IReadOnlyList<QuestionSnapshot> Questions,
    IReadOnlyList<string> Tags,
    int Version,
    DateTimeOffset CreatedAt);
public sealed record QuestionSnapshot(
    Guid Id,
    string Text,
    string? Explanation,
    Guid? SourceChunkId,
    IReadOnlyList<AnswerSnapshot> Answers);
public sealed record AnswerSnapshot(
    Guid Id,
    int Ordinal,
    string Text,
    bool IsCorrect);