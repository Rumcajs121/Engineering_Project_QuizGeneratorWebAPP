using LLMService.Commons.Models;
using Microsoft.Extensions.AI;

namespace LLMService.Features.GenerateQuiz;

public interface IGenerateQuizService
{
    Task<List<LlmQuiz>> GenerateQuiz(string task);
}

public class GenerateQuizService(IChatClient agent,IEmbeddingGenerator<string, Embedding<float>> embending) : IGenerateQuizService
{
    public Task<List<LlmQuiz>> GenerateQuiz(string task)
    {
        throw new NotImplementedException();
    }
}