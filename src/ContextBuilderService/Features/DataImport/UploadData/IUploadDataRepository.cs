namespace ContextBuilderService.ContextBuilder.UploadData;

public interface IUploadDataRepository
{
    Task<bool> UploadDataToBlob(IFormFile file);
    Task<byte[]> DownloadBlobData(string fileName);
    
}