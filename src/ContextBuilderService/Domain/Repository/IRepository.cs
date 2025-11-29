namespace ContextBuilderService.Domain.Repository;

public interface IRepository
{
    Task<byte[]> DownloadBlobData(string fileName);
    Task<bool> UploadDataToBlob(IFormFile file);
}