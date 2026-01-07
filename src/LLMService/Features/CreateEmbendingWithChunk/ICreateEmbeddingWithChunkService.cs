using LLMService.Commons.Models;
using LLMService.Infrastructure.Redis;
using Microsoft.Extensions.AI;
using LLMService.Infrastructure.VectorStore;

namespace LLMService.Features.CreateEmbendingWithChunk;

public interface ICreateEmbeddingWithChunkService
{
    Task<bool> CreateEmbedding(Guid documentId,CancellationToken ct);
}

public class CreateEmbeddingWithChunkService(IEmbeddingGenerator<string, Embedding<float>> embedding,IRedisDataRepository redisRepository,IVectorDataRepository vectorDataRepository):ICreateEmbeddingWithChunkService
{
    private async Task<List<ChunkEmbedding>> EmbeddingData(IEnumerable<Chunk> chunks,CancellationToken ct)
    {
        var result = new List<ChunkEmbedding>(chunks.Count());
        foreach (var chunk in chunks)
        {
            ct.ThrowIfCancellationRequested(); 
            var emb=await embedding.GenerateAsync(chunk.Content);
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
    public async Task<bool> CreateEmbedding(Guid documentId,CancellationToken ct)
    {
        var chunks=await redisRepository.GetChunksAsync(documentId);
        var embeddingChunks=await EmbeddingData(chunks,ct);
        var saveVectorData=await vectorDataRepository.SaveAsyncEmbedding(embeddingChunks);
        return saveVectorData;
    }
}               