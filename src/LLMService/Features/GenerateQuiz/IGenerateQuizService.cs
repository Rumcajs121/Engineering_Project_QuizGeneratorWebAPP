using System.Text.Json;
using LLMService.Commons.Models;
using LLMService.Infrastructure.LLMProvider;
using LLMService.Infrastructure.VectorStore;
using Microsoft.Extensions.AI;

namespace LLMService.Features.GenerateQuiz;

public interface IGenerateQuizService
{
    Task<LlmQuiz> GenerateQuiz(int k,int countQuestion,string question, IReadOnlyList<Guid> documentIds);
}

public class GenerateQuizService(IChatClient clientLLama,IVectorDataRepository repository) : IGenerateQuizService
{
    public async Task<LlmQuiz> GenerateQuiz(int k, int countQuestion, string question, IReadOnlyList<Guid> documentIds)
    {
        var searchQdrant = await repository.TopKChunk(k, question, documentIds);
        var messages = LLMPromptSettings.BuildQuizMessages(
            questionCount: countQuestion,
            topic: question,
            context:  searchQdrant);
        var options = new ChatOptions
        {
            ResponseFormat = ChatResponseFormat.ForJsonSchema<LlmQuiz>(
                schemaName: "quiz_schema",
                schemaDescription: "A quiz with questions and answers"
            )
        };

        var response = await clientLLama.GetResponseAsync(messages, options);
        
        var quiz = JsonSerializer.Deserialize<LlmQuiz>(response.Text, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        });

        return quiz ??  throw new InvalidOperationException("LLM returned invalid JSON.");
    }
}