using LLMService.Commons.Models;
using LLMService.Infrastructure.LLMProvider;
using LLMService.Infrastructure.VectorStore;
using Microsoft.Extensions.AI;
using Newtonsoft.Json;


namespace LLMService.Features.GenerateQuiz;

public interface IGenerateQuizService
{
    Task<LlmQuiz> GenerateQuiz(int k,int countQuestion,string question, IReadOnlyList<Guid> documentIds);
}

public class GenerateQuizService(IChatClient clientLLama,IVectorDataRepository repository) : IGenerateQuizService
{
    private static string CleanJsonResponse(string rawJson)
    {
        return rawJson
            . Trim()
            .Replace("```json", "")
            .Replace("```", "")
            .Trim();
    }
    public async Task<LlmQuiz> GenerateQuiz(int k, int countQuestion,string question, IReadOnlyList<Guid> documentIds)
    {
        var searchQdrant=await repository.TopKChunk(k,question,documentIds);
        var messages = LLMPromptSettings.BuildQuizMessages(
            questionCount: countQuestion,
            topic: question,
            context: searchQdrant);
        
        var response = await clientLLama.GetResponseAsync(messages);
        var jsonResponse = response.Text;
        var cleanJson=CleanJsonResponse(jsonResponse);
        var quiz = JsonConvert.DeserializeObject<LlmQuiz>(cleanJson);
        return quiz ?? throw new InvalidOperationException("LLM returned invalid JSON.");
    }
}