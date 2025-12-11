using Microsoft.Extensions.AI;
using OllamaSharp;

namespace LLMService.Infrastructure.LLMProvider;


public class ChatModelClient 
    : OllamaApiClient, IChatClient
{
    public ChatModelClient()
        : base(new Uri("http://localhost:11434"), "llama3")
    {
    }
}

public class EmbeddingModelClient 
    : OllamaApiClient, IEmbeddingGenerator<string, Embedding<float>>
{
    public EmbeddingModelClient()
        : base(new Uri("http://localhost:11434"), "nomic-embed-text")
    {
    }
}