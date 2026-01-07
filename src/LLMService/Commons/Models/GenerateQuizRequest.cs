namespace LLMService.Commons.Models;

public record GenerateQuizRequest(int K, int CountQuestion, string Question,IReadOnlyList<Guid> DocumentIds);