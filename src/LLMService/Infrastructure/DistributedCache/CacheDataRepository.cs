using LLMService.Commons.Models;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;

namespace LLMService.Infrastructure.DistributedCache;

public interface ICacheDataRepository
{
    Task<IEnumerable<Chunk>>  GetChunksAsync(Guid documentId);
} 
public class CacheDataRepository(IDistributedCache cache):ICacheDataRepository
{
    public async Task<IEnumerable<Chunk>> GetChunksAsync(Guid documentId)
    {
        var docKey = $"doc:{documentId}:chunks";
        var indexesJson = await cache.GetStringAsync(docKey);
        if (string.IsNullOrWhiteSpace(indexesJson))
            return Array.Empty<Chunk>();
        var indexes = JsonConvert.DeserializeObject<List<int>>(indexesJson);
        var result = new List<Chunk>(indexes.Count);
        foreach (var idx in indexes)
        {
            var key = $"chunk:{documentId}:{idx}";
            var chunkJson = await cache.GetStringAsync(key);

            if (string.IsNullOrWhiteSpace(chunkJson))
                continue;

            var chunk = JsonConvert.DeserializeObject<Chunk>(chunkJson);
            if (chunk is not null)
                result.Add(chunk);
        }
        return result.OrderBy(c => c.ChunkIndex).ToList();
    }
}