namespace LLMService.Commons.Models;

public record GenerateQuizParameter(int K, int CountQuestion, string Question,IReadOnlyList<Guid> DocumentIds,Guid ExternalId);