using LLMService.Commons.Models;
using LLMService.Infrastructure.LLMProvider;
using LLMService.Infrastructure.VectorStore;
using Microsoft.Extensions.AI;
using OllamaSharp.Models.Chat;
using ChatRole = Microsoft.Extensions.AI.ChatRole;

namespace LLMService.Features.GenerateQuiz;

public interface IGenerateQuizService
{
    Task<List<LlmQuiz>> GenerateQuiz(int k,string question, IReadOnlyList<Guid> documentIds);
}

public class GenerateQuizService(IChatClient agent,IVectorDataRepository repository) : IGenerateQuizService
{
    public async Task<List<LlmQuiz>> GenerateQuiz(int k, string question, IReadOnlyList<Guid> documentIds)
    {
        var searchQdrant=await repository.TopKChunk(k,question,documentIds);
        var messages = LLMPromptSettings.BuildQuizMessages(
            questionCount: 8,
            topic: question,
            context: searchQdrant);
        
        throw new NotImplementedException();
    }
}

// var response = await _chat.CompleteAsync(
//     new ChatRequest
//     {
//         Messages =
//         {
//             SystemPrompt(questionCount),
//             UserPrompt(topic, context)
//         }
//     },
//     cancellationToken: ct);