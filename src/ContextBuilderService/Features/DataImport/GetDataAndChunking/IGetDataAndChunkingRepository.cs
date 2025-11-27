namespace ContextBuilderService.Features.DataImport.GetDataAndChunking;

public interface IGetDataAndChunkingRepository
{
    Task<byte[]> DownloadBlobData(string fileName);
}