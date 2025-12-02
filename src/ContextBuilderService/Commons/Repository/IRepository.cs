using ContextBuilderService.Domain.DataImport;

namespace ContextBuilderService.Domain.Repository;

public interface IRepository
{
    Task<byte[]> DownloadBlobData(string fileName);
    Task<bool> UploadDataToBlob(IFormFile file);
    Task SaveChunkAsync(IEnumerable<Chunk> chunks);
}