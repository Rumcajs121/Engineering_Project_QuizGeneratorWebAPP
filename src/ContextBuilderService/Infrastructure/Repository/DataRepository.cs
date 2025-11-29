using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using ContextBuilderService.ContextBuilder.UploadData;
using ContextBuilderService.Domain.Repository;
using ContextBuilderService.Features.DataImport.GetDataAndChunking;

namespace ContextBuilderService.Infrastructure.DataImport.Repositories;

public class DataRepository : IRepository
{
    
    public async Task<bool> UploadDataToBlob(IFormFile file)
    {
        var connectionString =
            "DefaultEndpointsProtocol=https;AccountName=textnoteforquizblob;AccountKey=q733artWYaAg14eo151BuJAz9FDIOtG8prJ718PMhnfaELc9mXTpCSKfr6o9kIV/zNWLuG5O28QV+AStRVs8yA==;EndpointSuffix=core.windows.net";
        BlobServiceClient blobServiceClient = new BlobServiceClient(connectionString);
        var containerName = "dataquiz";
        BlobContainerClient containerClient = blobServiceClient.GetBlobContainerClient(containerName);
        await containerClient.CreateIfNotExistsAsync();
        BlobClient blobClient = containerClient.GetBlobClient(file.FileName);
        var blobHttpHeaders = new BlobHttpHeaders();
        blobHttpHeaders.ContentType = file.ContentType;
        await blobClient.UploadAsync(file.OpenReadStream(),blobHttpHeaders);

        return true;
    }

    public async Task<byte[]> DownloadBlobData(string fileName)
    {
        var connectionString =
            "DefaultEndpointsProtocol=https;AccountName=textnoteforquizblob;AccountKey=q733artWYaAg14eo151BuJAz9FDIOtG8prJ718PMhnfaELc9mXTpCSKfr6o9kIV/zNWLuG5O28QV+AStRVs8yA==;EndpointSuffix=core.windows.net";
        BlobServiceClient blobServiceClient = new BlobServiceClient(connectionString);
        var containerName = "dataquiz";
        BlobContainerClient containerClient = blobServiceClient.GetBlobContainerClient(containerName);
        await containerClient.CreateIfNotExistsAsync();
        BlobClient blobClient = containerClient.GetBlobClient(fileName);
        var donloadResponse=await blobClient.DownloadContentAsync();
        return donloadResponse.Value.Content.ToArray();
    }
}