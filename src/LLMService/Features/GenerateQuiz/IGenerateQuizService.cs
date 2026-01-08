using System.Text.Json;
using LLMService.Commons.Models;
using LLMService.Infrastructure.LLMProvider;
using LLMService.Infrastructure.Redis;
using LLMService.Infrastructure.VectorStore;
using Microsoft.Extensions.AI;
using Tiktoken;

namespace LLMService.Features.GenerateQuiz;

public interface IGenerateQuizService
{
    Task<LlmQuiz> GenerateQuiz(int k,int countQuestion,string question, IReadOnlyList<Guid> documentIds);
    Task<string> CreateJobAsync(GenerateQuizRequest request, CancellationToken ct = default);
}

public class GenerateQuizService(IChatClient clientLLama,IVectorDataRepository repository,IRedisDataRepository jobRepository,ILogger<GenerateQuizService> logger) : IGenerateQuizService
{
    private Encoder? _encoder;
    
    public async Task<LlmQuiz> GenerateQuiz(int k, int countQuestion, string question, IReadOnlyList<Guid> documentIds)
    {
        int questionTokens = CountTokens(question);
        logger.LogInformation($"Question tokens (from Qdrant): {questionTokens:N0}");
        var searchQdrant = await repository.TopKChunk(k, question, documentIds);
        
        // 2️⃣ Policz tokeny w kontekście (chunki z Qdrant)
        int contextTokens = CountTokens(searchQdrant);
        logger.LogInformation($"Context tokens (from Qdrant): {contextTokens:N0}");
        
        
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
        int promptTokens = CountTokens(messages);
        logger.LogInformation($"Prompt tokens (Question to Chat): {promptTokens:N0}");
        
        var response = await clientLLama.GetResponseAsync(messages, options);
        
        var quiz = JsonSerializer.Deserialize<LlmQuiz>(response.Text, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        });
        int responseTokens = CountTokens(response.Text);
        const int MAX_SAFE_TOKENS = 4096;
        double usagePercent = (double)promptTokens / MAX_SAFE_TOKENS * 100;
        bool isSafe = promptTokens < MAX_SAFE_TOKENS;
        int totalTokens = promptTokens + responseTokens;
        logger.LogInformation(
            "Token Analysis: Question={QuestionTokens}, Context={ContextTokens}, " +
            "Prompt={PromptTokens}, Response={ResponseTokens}, Total={TotalTokens}, " +
            "MaxSafe={MaxSafe}, Usage={Usage:F1}%, Safe={IsSafe}",
            questionTokens,
            contextTokens,
            promptTokens,
            responseTokens,
            totalTokens,
            MAX_SAFE_TOKENS,
            usagePercent,
            isSafe
        );
        return quiz ??  throw new InvalidOperationException("LLM returned invalid JSON.");
    }

    public async Task<string> CreateJobAsync(GenerateQuizRequest request, CancellationToken ct = default)
    {
        var jobId=await jobRepository.CreateJobAsync(request, ct);
        await jobRepository.EnqueueJobAsync(jobId);
        return jobId;
    }
    private int CountTokens(string text)
    {
        if (string.IsNullOrEmpty(text))
            return 0;
        
        _encoder ??= ModelToEncoder. For("gpt-4");
        
        var tokens = _encoder.Encode(text);
        return (int)(tokens.Count * 1.05);
    }
    private int CountTokens(IEnumerable<ChatMessage> messages)
    {
        if (_encoder == null)
        {
            _encoder = ModelToEncoder.For("gpt-4");
        }
        
        int total = 0;
        foreach (var message in messages)
        {
            if (!string. IsNullOrEmpty(message. Text))
            {
                var tokens = _encoder.Encode(message.Text);
                total += tokens.Count;
            }
            total += 4;
        }
        total = (int)(total * 1.05);
        return total + 2;
    }
}