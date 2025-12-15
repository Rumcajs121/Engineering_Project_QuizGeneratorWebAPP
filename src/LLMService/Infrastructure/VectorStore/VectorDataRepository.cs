using LLMService.Commons.Models;
using Microsoft.Extensions.AI;
using Qdrant.Client;
using Qdrant.Client.Grpc;

namespace LLMService.Infrastructure.VectorStore;

public interface IVectorDataRepository
{
    Task<bool> SaveAsyncEmbending(List<ChunkEmbedding> chunkEmbeddings);
    Task<string> TopKChunk(int k,string question, IReadOnlyList<Guid> documentIds);
}
public class VectorDataRepository(QdrantClient qdrantClient,IEmbeddingGenerator<string, Embedding<float>> embending):IVectorDataRepository
{
    
    private async Task EnsureCollectionAsync()
    {
        if (await qdrantClient.CollectionExistsAsync("chunksNotes")) return;

        await qdrantClient.CreateCollectionAsync(
            "chunksNotes",
            new VectorParams { Size = 768, Distance = Distance.Cosine });
    }
    public async Task<bool> SaveAsyncEmbending(List<ChunkEmbedding> chunkEmbeddings)
    {
        if (chunkEmbeddings is null || chunkEmbeddings.Count == 0) return true;
        await EnsureCollectionAsync();
        
        var points=chunkEmbeddings.Select(e=>new PointStruct
        {
            Id=new PointId{Uuid = e.Key.ToString()},
            Vectors = new Vectors
            {
                Vector = new Vector
                {
                    Data = { e.Embedding.ToArray() }
                }
            },
            Payload =
            {
                ["documentId"] = e.DocumentId.ToString(),
                ["chunkIndex"] = e.ChunkIndex,
                ["model"] = e.Model,
                ["text"] = e.ChunkText
            }
        }).ToList();
        await qdrantClient.UpsertAsync("chunksNotes", points);
        return true;
    }

    public async Task<string> TopKChunk(int k, string question, IReadOnlyList<Guid> documentIds)
    {
        var qEmbedding = await embending.GenerateAsync(question);
        var queryVector = qEmbedding.Vector.ToArray();
        var filter = new Filter();
        filter.Should.AddRange(
            documentIds.Select(id => new Condition
            {
                Field = new FieldCondition
                {
                    Key = "documentId",
                    Match = new Match { Text = id.ToString() }
                }
            })
        );
        var results = await qdrantClient.SearchAsync("chunksNotes",
            queryVector,
            limit: (ulong)k,
            filter: filter);
        return string.Join(
            "\n\n---\n\n",
            results
                .OrderByDescending(r => r.Score)
                .Select(r =>
                {
                    var chunkIndex = (int)r.Payload["chunkIndex"].IntegerValue;
                    var documentId = r.Payload["documentId"].StringValue;
                    var text = r.Payload["text"].StringValue;
                    return $"[Document {documentId} | Chunk {chunkIndex}]\n{text}";
                })
        );
    }
}