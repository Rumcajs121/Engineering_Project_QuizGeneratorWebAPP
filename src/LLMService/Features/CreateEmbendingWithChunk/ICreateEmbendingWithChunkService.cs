using LLMService.Commons.Models;
using Microsoft.Extensions.AI;

using LLMService.Infrastructure.DistributedCache;
using LLMService.Infrastructure.VectorStore;

namespace LLMService.Features.CreateEmbendingWithChunk;

public interface ICreateEmbendingWithChunkService
{
    Task<bool> CreateEmbedding(Guid documentId);
}

public class CreateEmbendingWithChunkService(IEmbeddingGenerator<string, Embedding<float>> embedding,ICacheDataRepository cacheRepository,IVectorDataRepository vectorDataRepository):ICreateEmbendingWithChunkService
{
    private async Task<List<ChunkEmbedding>> EmbeddingData(IEnumerable<Chunk> chunks)
    {
        var result = new List<ChunkEmbedding>(chunks.Count());
        foreach (var chunk in chunks)
        {
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
    public async Task<bool> CreateEmbedding(Guid documentId)
    {
        var chunks=await cacheRepository.GetChunksAsync(documentId);
        var embeddingChunks=await EmbeddingData(chunks);
        var saveVectorData=await vectorDataRepository.SaveAsyncEmbending(embeddingChunks);
        return saveVectorData;
    }
}               