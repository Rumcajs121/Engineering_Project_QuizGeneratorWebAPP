using BuildingBlocks.Redis;
using LLMService.Commons.Models;
using LLMService.Features.GenerateQuiz;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
using StackExchange.Redis;

namespace LLMService.Infrastructure.Redis;

public interface IRedisDataRepository
{
    Task<IEnumerable<Chunk>>  GetChunksAsync(Guid documentId);
    Task<string> CreateJobAsync(GenerateQuizRequest request, CancellationToken ct = default);
    Task<QuizJob? > GetJobAsync(string jobId, CancellationToken ct = default);
    Task UpdateJobAsync(QuizJob job, CancellationToken ct = default);
    Task EnqueueJobAsync(string jobId);
    Task<string? > DequeueJobAsync();
    Task AckJobAsync(string jobId);
} 
public class RedisDataRepository(IDistributedCache cache, IConnectionMultiplexer redis) : IRedisDataRepository
{
    private readonly IDatabase _db = redis.GetDatabase();
    private const string QueueKey = "queue:quizjobs";
    private const string ProcessingKey = "queue:quizjobs:processing";
    public async Task<IEnumerable<Chunk>> GetChunksAsync(Guid documentId)
    {
        var indexesJson = await cache.GetStringAsync(RedisConfig.DocChunks(documentId));
        if (string.IsNullOrWhiteSpace(indexesJson))
            return Array.Empty<Chunk>();
        var indexes = JsonConvert.DeserializeObject<List<int>>(indexesJson);
        if (indexes == null || indexes.Count == 0)
            return Array.Empty<Chunk>();
        var result = new List<Chunk>(indexes.Count);
        foreach (var idx in indexes)
        {
            var chunkJson = await cache.GetStringAsync(RedisConfig.Chunk(documentId, idx));

            if (string.IsNullOrWhiteSpace(chunkJson))
                continue;

            var chunk = JsonConvert.DeserializeObject<Chunk>(chunkJson);
            if (chunk is not null)
                result.Add(chunk);
        }
        return result.OrderBy(c => c.ChunkIndex).ToList();
    }

    //QUEUE FOR BACKGROUND-SERVICE
    public async Task<string> CreateJobAsync(GenerateQuizRequest request, CancellationToken ct = default)
    {
        ArgumentNullException.ThrowIfNull(request);
        var job = new QuizJob
        {
            JobId = Guid. NewGuid().ToString(),
            Status = QuizJobStatus.Pending,
            Request = request,
            CreatedAt = DateTime.UtcNow
        };
        await SaveJobAsync(job, ct);
        return job.JobId;
    }

    public async Task<QuizJob?> GetJobAsync(string jobId, CancellationToken ct = default)
    {
        if (string.IsNullOrWhiteSpace(jobId))
            return null;
        var jobKey = GetJobKey(jobId);
        var jobJson = await cache.GetStringAsync(jobKey,ct);
        if (string.IsNullOrEmpty(jobJson))
            return null;
        return JsonConvert.DeserializeObject<QuizJob>(jobJson);
    }

    public async Task UpdateJobAsync(QuizJob job, CancellationToken ct = default)
    {
        ArgumentNullException.ThrowIfNull(job);
        job.CompletedAt = job.Status is QuizJobStatus.Sent or QuizJobStatus.Failed 
            ? DateTime.UtcNow 
            : null;
        await SaveJobAsync(job, ct);
    }

    public async Task EnqueueJobAsync(string jobId)
    {
        if (string.IsNullOrWhiteSpace(jobId)) 
            throw new ArgumentException("JobId cannot be null or empty", nameof(jobId));
        await _db.ListLeftPushAsync(QueueKey, jobId);
    }

    public async Task<string?> DequeueJobAsync()
    {
        var result = await _db.ExecuteAsync(
            "BRPOPLPUSH",
            QueueKey,
            ProcessingKey,
            10
        );
        if (result.IsNull)
            return null;
        return result.ToString();
    }

    public Task AckJobAsync(string jobId)
    {
        return _db.ListRemoveAsync(ProcessingKey, jobId, count: 1);
    }

    private async Task SaveJobAsync(QuizJob job, CancellationToken ct)
    {
        var jobKey = GetJobKey(job.JobId);
        var jobJson = JsonConvert.SerializeObject(job);
        var options = new DistributedCacheEntryOptions
        {
            AbsoluteExpirationRelativeToNow = TimeSpan.FromHours(24)
        };
        await cache.SetStringAsync(jobKey, jobJson, options, ct);
    }
    private static string GetJobKey(string jobId) => $"job:quiz:{jobId}";
}