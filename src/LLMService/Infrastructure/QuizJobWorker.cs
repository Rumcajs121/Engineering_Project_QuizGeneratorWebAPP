using LLMService.Commons.Models;
using LLMService.Infrastructure.Redis;

namespace LLMService.Infrastructure;

public class QuizJobWorker(
    IServiceScopeFactory scopeFactory,
    ILogger<QuizJobWorker> logger,
    IHttpClientFactory httpClientFactory,
    IWorkflowGenerateQuizByLlm workflowLlm) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        const int retryCount = 2;
        while (!stoppingToken.IsCancellationRequested)
        {
            using var scope = scopeFactory.CreateScope();

            var repository = scope.ServiceProvider
                .GetRequiredService<IRedisDataRepository>();
            string? jobId = null;
            try
            {
                LlmQuiz? quiz = null;
                IReadOnlyList<Guid> documentId = null;
                Guid externalId;
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
                try
                {
                    quiz = await workflowLlm.GenerateQuizPipeline(job.Parameter.K, job.Parameter.CountQuestion,
                        job.Parameter.Question, job.Parameter.DocumentIds,stoppingToken);
                }
                catch (Exception ex)
                {
                    job.RetryCount = (job.RetryCount ?? 0) + 1;
                    if (job.RetryCount < retryCount)
                    {
                        job.Error = ex.Message;
                        job.Status = QuizJobStatus.Pending;
                        logger.LogWarning("Job {JobId} failed (attempt {Attempt}/{MaxRetries}). Requeuing.", 
                            jobId, job.RetryCount, retryCount);
                        await repository.UpdateJobAsync(job, stoppingToken);
                        await repository.RequeueJobAsync(jobId);
                        continue;
                    }else
                    {
                        logger.LogCritical("Job {JobId} permanently failed after 2 attempts", jobId);
                        job.Status = QuizJobStatus.Failed;
                        await repository.UpdateJobAsync(job, stoppingToken);
                        await repository.AckJobAsync(jobId);
                        continue;
                    }
                }
                if (quiz == null)
                {
                    job.Status = QuizJobStatus.Failed;
                    job.Error = "Quiz is null";
                    await repository.UpdateJobAsync(job, stoppingToken);
                    await repository.AckJobAsync(jobId);
                    continue;
                }

                job.Result = quiz;
                job.Status = QuizJobStatus.Generated;
                await repository.UpdateJobAsync(job, stoppingToken);
                logger.LogInformation("Job {JobId} generated successfully", jobId);

                //From QuizService
                var httpContext = httpClientFactory.CreateClient("llmtoquizcomunications");
                documentId = job.Parameter.DocumentIds;
                externalId = job.Parameter.ExternalId;
                var quizContext = FactoryBuildQuizDto(quiz, documentId.ToList(), externalId);
                var payload = new { createQuizDto = quizContext };
                var response = await httpContext.PostAsJsonAsync("/quiz", payload, cancellationToken: stoppingToken);
                if (!response.IsSuccessStatusCode)
                {
                    var body = await response.Content.ReadAsStringAsync(stoppingToken);
                    logger.LogError($"Parameter failed: {response.StatusCode} - {response.Content} - {response}");
                    job.Status = QuizJobStatus.Failed;
                    job.Error = $"QuizService HTTP {(int)response.StatusCode} {response.StatusCode}. Body: {body}";
                    await repository.UpdateJobAsync(job, stoppingToken);
                    await repository.AckJobAsync(jobId);
                    continue;
                }

                var quizId =
                    await response.Content.ReadFromJsonAsync<CreateQuizResponse>(cancellationToken: stoppingToken);
                logger.LogInformation($"Parameter status {response.StatusCode} to create quiz {quizId.Id} succes");
                job.Status = QuizJobStatus.Sent;
                await repository.UpdateJobAsync(job, stoppingToken);
                await repository.AckJobAsync(jobId);
            }
            catch (OperationCanceledException)when (stoppingToken.IsCancellationRequested)
            {
                logger.LogCritical("QuizJobWorker is stopping");
                return;
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Unexpected worker error (jobId={JobId}). Job will be failed.", jobId);
                if (! string.IsNullOrEmpty(jobId))
                {
                    try
                    {
                        var job = await repository.GetJobAsync(jobId, stoppingToken);
                        if (job != null)
                        {
                            job.Status = QuizJobStatus.Failed;
                            job.Error = $"Unexpected error:  {ex.Message}";
                            await repository.UpdateJobAsync(job, stoppingToken);
                        }
                        await repository.AckJobAsync(jobId);
                    }
                    catch (Exception e)
                    {
                        logger.LogError(e, "Failed to clean up job {JobId}. Manual intervention required.", jobId);
                    }
                }
                await Task.Delay(300, stoppingToken);
            }
        }
    }

    private static RequestQuizDto FactoryBuildQuizDto(LlmQuiz generateQuizByLlm, List<Guid> documentId, Guid externalId)
    {
        var buildQuizContext = new RequestQuizDto
        (
            QuizId: Guid.NewGuid(),
            QuizStatus: "Generating",
            ExternalId: externalId,
            SourceId: documentId,
            Title: generateQuizByLlm.Title,
            CreatedAt: DateTime.Now,
            Question: generateQuizByLlm.Questions.Select(q => new RequestQuestionQuizDto
            (
                QuestionId: Guid.NewGuid(),
                Text: q.Text,
                Explanation: q.Explanation,
                SourceChunkId: Guid.NewGuid(),
                Answer: q.Answers.Select(a => new RequestAnswerQuizDto
                (
                    AnswerId: Guid.NewGuid(),
                    Ordinal: a.Ordinal,
                    Text: a.Text,
                    IsCorrect: a.IsCorrect
                )).ToList()
            )).ToList(),
            Tag: generateQuizByLlm.Tags
        );

        return buildQuizContext;
    }
}