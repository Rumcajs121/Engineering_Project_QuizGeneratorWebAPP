using Microsoft.Extensions.VectorData;

namespace LLMService.Commons.Models;

public class ChunkEmbedding
{
    [VectorStoreKey]
    public Guid Key { get; set; }=Guid.NewGuid();
    [VectorStoreData]
    public Guid DocumentId { get; set; }
    [VectorStoreData]
    public int ChunkIndex { get; set; }
    [VectorStoreData]
    public string ChunkText { get; set; }
    [VectorStoreData]
    public string Model { get; set; } = "nomic-embed-text";
    [VectorStoreVector(Dimensions: 768, DistanceFunction = DistanceFunction.CosineSimilarity)]
    public ReadOnlyMemory<float> Embedding { get; set; }
}
