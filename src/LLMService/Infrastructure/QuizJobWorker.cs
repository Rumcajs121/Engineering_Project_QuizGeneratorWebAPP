using LLMService.Commons.Models;
using LLMService.Features.GenerateQuiz;
using LLMService.Infrastructure.Redis;

namespace LLMService.Infrastructure;

public class QuizJobWorker(IServiceScopeFactory scopeFactory, ILogger<QuizJobWorker> logger) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            using var scope = scopeFactory.CreateScope();

            var repository = scope.ServiceProvider
                .GetRequiredService<IRedisDataRepository>();

            var service = scope.ServiceProvider
                .GetRequiredService<IGenerateQuizService>();

            while (!stoppingToken.IsCancellationRequested)
            {
                string? jobId = null;
                try
                {
                    jobId = await repository.DequeueJobAsync();
                    if (jobId == null)
                    {
                        continue;
                    }

                    logger.LogInformation("Processing job {JobId}", jobId);
                    var job = await repository.GetJobAsync(jobId, stoppingToken);
                    if (job == null)
                    {
                        logger.LogWarning("Job {JobId} not found in Redis", jobId);
                        await repository.AckJobAsync(jobId);
                        continue;
                    }

                    job.Status = QuizJobStatus.Running;
                    await repository.UpdateJobAsync(job, stoppingToken);
                    var quiz = await service.GenerateQuiz(
                        job.Request.K,
                        job.Request.CountQuestion,
                        job.Request.Question,
                        job.Request.DocumentIds);
                    job.Result = quiz;
                    job.Status = QuizJobStatus.Generated;
                    logger.LogInformation("Job {JobId} starting generated", jobId);
                    
                    await repository.UpdateJobAsync(job, stoppingToken);
                    if (job.Result == null)
                    {
                        job.Status = QuizJobStatus.Failed;
                        await repository.UpdateJobAsync(job, stoppingToken);
                        
                        logger.LogWarning("Job {JobId} failed — quiz is null", jobId);
                        //TODO: In the future requeue Job.
                        await repository.AckJobAsync(jobId);
                        continue;
                    }

                    logger.LogInformation("Job {JobId} generated successfully", jobId);
                    //TODO: Send QuizService to create quiz 
                    job.Status = QuizJobStatus.Sent;
                    await repository.UpdateJobAsync(job, stoppingToken);
                    await repository.AckJobAsync(jobId);
                    

                }
                catch (OperationCanceledException)
                {
                    logger.LogInformation("QuizJobWorker is stopping");
                    break;
                }
            }
        }
    }
}