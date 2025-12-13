using LLMService.Commons.Models;
using Microsoft.Extensions.AI;
using OllamaSharp.Models;
using System.Linq;
namespace LLMService.Features.CreateEmbendingWithChunk;

public interface ICreateEmbendingWithChunkService
{
    Task<List<ChunkEmbedding>> CreateEmbending(IEnumerable<Chunk> chunks);
}

public class CreateEmbendingWithChunkService(IEmbeddingGenerator<string, Embedding<float>> embending):ICreateEmbendingWithChunkService
{
    public async Task<List<ChunkEmbedding>> CreateEmbending(IEnumerable<Chunk> chunks)
    {
        var result = new List<ChunkEmbedding>(chunks.Count());
        foreach (var chunk in chunks)
        {
            var emb=await embending.GenerateAsync(chunk.Content);
            result.Add(new ChunkEmbedding
            {
                DocumentId = chunk.DocumentId,
                ChunkIndex = chunk.ChunkIndex,
                ChunkText = chunk.Content,
                Embedding = emb.Vector
            });
        }
        return result;
    }
}               