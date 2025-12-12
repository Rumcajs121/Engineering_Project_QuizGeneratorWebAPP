namespace LLMService.Commons.Models;

public sealed class LlmQuiz
{
    public string Title { get; init; } = default!;
    public IReadOnlyList<string> Tags { get; init; } = [];
    public IReadOnlyList<LlmQuestion> Questions { get; init; } = [];
}

public sealed class LlmQuestion
{
    public string Text { get; init; } = default!;
    public string Explanation { get; init; } = default!;
    public int SourceChunkIndex { get; init; }
    public IReadOnlyList<LlmAnswer> Answers { get; init; } = [];
}

public sealed class LlmAnswer
{
    public int Ordinal { get; init; }
    public string Text { get; init; } = default!;
    public bool IsCorrect { get; init; }
}