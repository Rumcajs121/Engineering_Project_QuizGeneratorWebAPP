using System.ComponentModel;
using System.Text.Json.Serialization;

namespace LLMService.Commons.Models;

public sealed class LlmQuiz
{
    [Description("Quiz title in Polish. Non-empty. Short and specific to the topic (max ~80 chars).")]
    [JsonPropertyName("title")]
    public string Title { get; init; } = default!;
    
    [Description("Tags in Polish. Provide 3–8 concise tags that summarize the quiz topic.")]
    [JsonPropertyName("tags")]
    public IReadOnlyList<string> Tags { get; init; } = [];
    
    [Description("List of quiz questions. Must contain exactly the requested number of questions.")]
    [JsonPropertyName("questions")]
    public IReadOnlyList<LlmQuestion> Questions { get; init; } = [];
}

public sealed class LlmQuestion
{
    [Description("Question text in Polish. Must be answerable using ONLY the provided context. Be precise and unambiguous.")]
    [JsonPropertyName("text")]
    public string Text { get; init; } = default! ;
    
    [Description("1–2 sentences in Polish. A subtle hint/clue from the context that helps reasoning, but MUST NOT reveal, restate, or obviously point to the correct answer. Non-empty (avoid generic filler).")]
    [JsonPropertyName("explanation")]
    public string Explanation { get; init; } = default!;
    
    [Description("Index X from the context header [Chunk X] that supports this question and its correct answer. Must be present for every question.")]
    [JsonPropertyName("sourceChunkIndex")]
    public int SourceChunkIndex { get; init; }
    
    [Description("Exactly 4 answers. Ordinals must be unique and in range 0..3. SINGLE-ANSWER rule: exactly one answer must have isCorrect=true, the other three must be false. Avoid overlapping options.")]
    [JsonPropertyName("answers")]
    public IReadOnlyList<LlmAnswer> Answers { get; init; } = [];
}

public sealed class LlmAnswer
{
    [Description("Answer option ordinal. Must be a non-negative integer (0, 1, 2, ...), unique within the question.")]
    [JsonPropertyName("ordinal")]
    public int Ordinal { get; init; }
    
    [Description("Answer text in Polish. Non-empty. Must be clearly distinct from the other options.")]
    [JsonPropertyName("text")]
    public string Text { get; init; } = default!;
    
    [Description("True only for the single correct option in this question. Exactly one true per question.")]
    [JsonPropertyName("isCorrect")]
    public bool IsCorrect { get; init; }
}