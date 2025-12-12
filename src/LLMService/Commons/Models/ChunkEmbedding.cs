namespace LLMService.Commons.Models;

public record ChunkEmbedding
(
    Guid DocumentId,
    int ChunkIndex,
    string Model,              
    float[] Vector
);