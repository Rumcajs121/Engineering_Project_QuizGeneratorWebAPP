namespace ContextBuilderService.Domain.DataImport;

public record Chunk(
    Guid DocumentId,
    int ChunkIndex,
    int TotalChunks,
    string FileName,
    string Content
    );