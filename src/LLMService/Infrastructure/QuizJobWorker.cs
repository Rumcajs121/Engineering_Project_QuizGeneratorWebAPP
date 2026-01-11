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
                    documentId = job.Request.DocumentIds; //TODO  BACGROUDSERVICE do Wyjasnienia
                    logger.LogInformation("Job {JobId} starting generated", jobId);
                    
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
                    
                    //Request do QuizService
                    var quizContext = FactoryBuildQuizDto(quiz,documentId.First()); 
                    var payload = new { createQuizDto = quizContext };
                    var response=await httpContext.PostAsJsonAsync("/quiz",payload, cancellationToken: stoppingToken);
                    if (!response.IsSuccessStatusCode)
                    {
                            var body=await response.Content.ReadAsStringAsync(stoppingToken);
                            logger.LogError($"Request failed: {response.StatusCode} - {response.Content} - {response}");
                            continue;
                    }
                    var quizId=await response.Content.ReadFromJsonAsync<CreateQuizResponse>(cancellationToken: stoppingToken);
                    logger.LogInformation($"Request status {response.StatusCode} to create quiz {quizId.Id} succes");
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

    private static RequestQuizDto FactoryBuildQuizDto(LlmQuiz generateQuizByLlm, Guid documentId )
    {
        var buildQuizContext = new RequestQuizDto
        (
            QuizId: Guid.NewGuid(), //TODO: BACGROUDSERVICE  Do Wyjaśnienia !!
            QuizStatus: "Generating",
            SourceId: documentId,
            Title: generateQuizByLlm.Title,
            CreatedAt: DateTime.Now,
            Question: generateQuizByLlm.Questions.Select(q => new RequestQuestionQuizDto
            (
                QuestionId: Guid.NewGuid(), 
                Text: q.Text,
                Explanation: q.Explanation,
                SourceChunkId: Guid.NewGuid(),//TODO: Do Wyjaśnienia !!
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