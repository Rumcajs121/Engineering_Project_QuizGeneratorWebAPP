using QuizService.Domain.Enums;
using QuizService.Domain.ValuesObject;

namespace QuizService.Domain.Models.Quiz.Snapshots;

public sealed record QuizSnapshot(
    QuizId QuizId,
    string QuizName,
    string Status,
    Guid ExternalId,
    List<Guid> SourceId,
    string? ShortDescription,
    IReadOnlyList<QuestionSnapshot> Questions,
    IReadOnlyList<string> Tags);
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