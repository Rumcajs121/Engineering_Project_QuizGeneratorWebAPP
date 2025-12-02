using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using ContextBuilderService.Domain.DataImport;
using ContextBuilderService.Domain.Repository;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;

namespace ContextBuilderService.Infrastructure.Repository;

public class DataRepository(IDistributedCache cache,IConfiguration configuration) : IRepository
{
    public async Task<bool> UploadDataToBlob(IFormFile file)
    {
         BlobServiceClient blobServiceClient = new BlobServiceClient(configuration.GetConnectionString("BlobStorage"));
        var containerName = "dataquiz";
        BlobContainerClient containerClient = blobServiceClient.GetBlobContainerClient(containerName);
        await containerClient.CreateIfNotExistsAsync();
        var blobName = $"{Guid.NewGuid()}";
        BlobClient blobClient = containerClient.GetBlobClient(blobName);
        var blobHttpHeaders = new BlobHttpHeaders();
        blobHttpHeaders.ContentType = file.ContentType;
        await blobClient.UploadAsync(file.OpenReadStream(), blobHttpHeaders);

        return true;
    }
    
    public async Task SaveChunkAsync(IEnumerable<Chunk> chunks)
    {
        Guid? documentId = null;
        var indexes = new List<int>();

        foreach (var chunk in chunks)
        {
            documentId ??= chunk.DocumentId;
            indexes.Add(chunk.ChunkIndex);

            var key = $"chunk:{chunk.DocumentId}:{chunk.ChunkIndex}";
            var json = JsonConvert.SerializeObject(chunk);

            await cache.SetStringAsync(key, json, new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromHours(24)
            });
        }
        if (documentId is not null)
        {
            var docKey = $"doc:{documentId}:chunks";
            var indexesJson = JsonConvert.SerializeObject(indexes);

            await cache.SetStringAsync(docKey, indexesJson, new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromHours(24)
            });
        }
    }

    public async Task<byte[]> DownloadBlobData(string fileName)
    {
        BlobServiceClient blobServiceClient = new BlobServiceClient(configuration.GetConnectionString("BlobStorage"));
        var containerName = "dataquiz";
        BlobContainerClient containerClient = blobServiceClient.GetBlobContainerClient(containerName);
        await containerClient.CreateIfNotExistsAsync();
        BlobClient blobClient = containerClient.GetBlobClient(fileName);
        var donloadResponse = await blobClient.DownloadContentAsync();
        return donloadResponse.Value.Content.ToArray();
    }
}