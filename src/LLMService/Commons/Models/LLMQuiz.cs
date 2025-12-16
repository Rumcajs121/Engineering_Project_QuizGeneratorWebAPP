using System.ComponentModel;
using System.Text.Json.Serialization;

namespace LLMService.Commons.Models;

public sealed class LlmQuiz
{
    [Description("Title of the quiz")]
    [JsonPropertyName("title")]
    public string Title { get; init; } = default!;
    
    [Description("Tags for categorizing the quiz")]
    [JsonPropertyName("tags")]
    public IReadOnlyList<string> Tags { get; init; } = [];
    
    [Description("List of quiz questions")]
    [JsonPropertyName("questions")]
    public IReadOnlyList<LlmQuestion> Questions { get; init; } = [];
}

public sealed class LlmQuestion
{
    [Description("The question text")]
    [JsonPropertyName("text")]
    public string Text { get; init; } = default! ;
    
    [Description("Explanation of the correct answer")]
    [JsonPropertyName("explanation")]
    public string Explanation { get; init; } = default!;
    
    [Description("Index of the source chunk from context")]
    [JsonPropertyName("sourceChunkIndex")]
    public int SourceChunkIndex { get; init; }
    
    [Description("List of possible answers")]
    [JsonPropertyName("answers")]
    public IReadOnlyList<LlmAnswer> Answers { get; init; } = [];
}

public sealed class LlmAnswer
{
    [Description("Order of the answer (1-4)")]
    [JsonPropertyName("ordinal")]
    public int Ordinal { get; init; }
    
    [Description("The answer text")]
    [JsonPropertyName("text")]
    public string Text { get; init; } = default!;
    
    [Description("Whether this is the correct answer")]
    [JsonPropertyName("isCorrect")]
    public bool IsCorrect { get; init; }
}