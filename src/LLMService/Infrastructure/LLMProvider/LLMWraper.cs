using Microsoft.Extensions.AI;
using OllamaSharp;

namespace LLMService.Infrastructure.LLMProvider;


public class ChatModelClient 
    : OllamaApiClient, IChatClient
{
    public ChatModelClient(IConfiguration configuration)
        : base(CreateHttpClient(configuration), "llama3.1-quiz")
    {
    }

    private static HttpClient CreateHttpClient(IConfiguration configuration)
    {
        return new HttpClient
        {
            BaseAddress = new Uri("http://localhost:11434"),
            Timeout = TimeSpan.FromMinutes(10)
        };
    }
    // public ChatModelClient()
    //     : base(new Uri("http://localhost:11434"), "llama3.1")
    // {
    //     
    // }
}

public class EmbeddingModelClient 
    : OllamaApiClient, IEmbeddingGenerator<string, Embedding<float>>
{
    public EmbeddingModelClient()
        : base(new Uri("http://localhost:11434"), "nomic-embed-text")
    {
    }
}