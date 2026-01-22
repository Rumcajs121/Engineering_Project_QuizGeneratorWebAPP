using System.Text.Json;
using BuildingBlocks.Security.ClientToService.CurrentUser;
using LLMService.Commons.Models;
using LLMService.Infrastructure.LLMProvider;
using LLMService.Infrastructure.Redis;
using LLMService.Infrastructure.VectorStore;
using Microsoft.Extensions.AI;
using Tiktoken;

namespace LLMService.Features.GenerateQuiz;

public interface IGenerateQuizService
{
    Task<string> CreateJobAndAddQueue(int k, int countQuestion,string question, IReadOnlyList<Guid> documentIds, CancellationToken ct = default);
}

public class GenerateQuizService(IRedisDataRepository jobRepository, ICurrentUser currentUser) : IGenerateQuizService
{

    public async Task<string> CreateJobAndAddQueue(int k, int countQuestion,string question, IReadOnlyList<Guid> documentIds, CancellationToken ct = default)
    {
        var externalId = Guid.Parse(currentUser.Subject);
        var context=new GenerateQuizParameter(k,countQuestion,question,documentIds,externalId);
        var jobId=await jobRepository.CreateJobAsync(context, ct);
        await jobRepository.EnqueueJobAsync(jobId);
        return jobId;
    }
}