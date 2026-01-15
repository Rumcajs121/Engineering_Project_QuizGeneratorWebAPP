using LLMService.Commons.Models;
using LLMService.Features.GenerateQuiz;
using LLMService.Infrastructure.Redis;

namespace LLMService.Infrastructure;

public class QuizJobWorker(IServiceScopeFactory scopeFactory, ILogger<QuizJobWorker> logger,IHttpClientFactory httpClientFactory) : BackgroundService
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
                 IReadOnlyList<Guid> documentId=null;
                 Guid externalId;
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
                    //TODO Validation !!
                    var quiz = await service.GenerateQuiz(
                        job.Parameter.K,
                        job.Parameter.CountQuestion,
                        job.Parameter.Question,
                        job.Parameter.DocumentIds);
                    job.Result = quiz;
                    job.Status = QuizJobStatus.Generated;
                    documentId = job.Parameter.DocumentIds; 
                    externalId=job.Parameter.ExternalId;
                    logger.LogInformation("Job {JobId} starting generated", jobId);
                    
                    //TODO: CreateValidationMethod input parameter QuizObject 
                    
                    await repository.UpdateJobAsync(job, stoppingToken);
                    if (job.Result == null)
                    {
                        job.Status = QuizJobStatus.Failed;
                        await repository.UpdateJobAsync(job, stoppingToken);
                        
                        logger.LogWarning("Job {JobId} failed — quiz is null", jobId);
                        //TODO: Retry Job.
                        await repository.AckJobAsync(jobId);
                        continue;
                    }
                    
                    logger.LogInformation("Job {JobId} generated successfully", jobId);
                    var httpContext=httpClientFactory.CreateClient("llmtoquizcomunications");
                    
                    //Parameter do QuizService
                    var quizContext = FactoryBuildQuizDto(quiz,documentId.ToList(),externalId); 
                    var payload = new { createQuizDto = quizContext };
                    var response=await httpContext.PostAsJsonAsync("/quiz",payload, cancellationToken: stoppingToken);
                    if (!response.IsSuccessStatusCode)
                    {
                            var body=await response.Content.ReadAsStringAsync(stoppingToken);
                            logger.LogError($"Parameter failed: {response.StatusCode} - {response.Content} - {response}");
                            continue;
                    }
                    var quizId=await response.Content.ReadFromJsonAsync<CreateQuizResponse>(cancellationToken: stoppingToken);
                    logger.LogInformation($"Parameter status {response.StatusCode} to create quiz {quizId.Id} succes");
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
    //TODO: change for List<Guid> Guid documentIds
    private static RequestQuizDto FactoryBuildQuizDto(LlmQuiz generateQuizByLlm, List<Guid> documentId,Guid  externalId )
    {
        
        var buildQuizContext = new RequestQuizDto
        (
            //TODO: Add UserID
            QuizId: Guid.NewGuid(), 
            QuizStatus: "Generating",
            ExternalId:externalId,
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