using System.Text.Json;
using FluentValidation;
using LLMService.Commons.Models;
using LLMService.Infrastructure.LLMProvider;
using LLMService.Infrastructure.VectorStore;
using Microsoft.Extensions.AI;

namespace LLMService.Infrastructure;

public interface IWorkflowGenerateQuizByLlm
{
    Task<LlmQuiz> GenerateQuizPipeline(string contextQdrant, int countQuestion, string question,CancellationToken ct);
}
public record LlmRetryRequest(int QuestionCount, string Question, string Errors, string? IncorrectQuiz, string?  ContextRag);

public class WorkflowGenerateQuizByLlm(IChatClient clientLLama,ILogger<WorkflowGenerateQuizByLlm> logger):IWorkflowGenerateQuizByLlm
{
        public async Task<LlmQuiz> GenerateQuizPipeline(string contextQdrant, int countQuestion, string question,CancellationToken ct)
        {
            const int maxAttempts = 2;
            // First Answear.
            var messages = LLMPromptSettings.BuildQuizMessages(
                questionCount: countQuestion,
                topic: question,
                context:  contextQdrant);
            var options = new ChatOptions
            {
                ResponseFormat = ChatResponseFormat.ForJsonSchema<LlmQuiz>(
                    schemaName: "quiz_schema",
                    schemaDescription: "A quiz with questions and answers"
                )
            };
            var response = await clientLLama.GetResponseAsync(messages, options);
            //Logger token
            var totalTokens = response.Usage.TotalTokenCount;
            var percentusage = (totalTokens * 100) / 8192;
            logger.LogInformation(
                $"MessagesCount:  InPut Token: {response.Usage.InputTokenCount} tokens | " +
                $"Output Token:{response.Usage.OutputTokenCount} tokens |" +
                $"UsagePercentage: (~{percentusage}% of 8192 limit)"
            );
            for (int attempt = 1; attempt <= maxAttempts; attempt++)
            {
                // First Check.
                if (!TryDeserializeLlmQuiz(response, out var quiz, out var parseError))
                {
                    if (attempt == maxAttempts)
                    {
                        throw new InvalidOperationException($"LLM failed to return valid JSON after {maxAttempts} attempts.  Last error: {parseError}");
                    }
                    var retry = new LlmRetryRequest(countQuestion, question, parseError, response.Text, ContextRag: null);
                    response = await ReturnAnswear(retry);
                    
                    
                    await Task.Delay(500 * attempt,ct);
                    continue;
                    
                }
                
                //Do Validation Model
                var validationMessages = LlmValidationResult(quiz);
                //Thread Check
                if (validationMessages.Count > 0)
                {
                    
                    if (attempt == maxAttempts)
                    {
                        throw new ValidationException($"LLM failed validation after {maxAttempts} attempts:  {string.Join(", ", validationMessages)}");
                    }
                    var err = string.Join("\n", validationMessages.Select(e => $"- {e}"));
                    var retry = new LlmRetryRequest(countQuestion, question, err, JsonSerializer.Serialize(quiz), ContextRag: null);
                    response = await ReturnAnswear(retry);
                    await Task.Delay(500 * attempt, ct);
                    continue;
                }
                // Second Check
                if (quiz.Questions.Count < countQuestion)
                {
                    if (attempt == maxAttempts)
                    {
                        throw new InvalidOperationException($"LLM failed to return correct question count after {maxAttempts} attempts. Expected {countQuestion}, got {quiz.Questions.Count}");
                    }
                    var err = $"Questions.Count mismatch. Expected {countQuestion}, got {quiz.Questions.Count}. Missing {countQuestion - quiz.Questions.Count}.";
                    var retry = new LlmRetryRequest(countQuestion, question, err, JsonSerializer.Serialize(quiz), contextQdrant);
                    response = await ReturnAnswear(retry);
                    await Task.Delay(500 * attempt, ct);
                    continue;
                }
                return quiz;
            }
            throw new Exception("LLM failed to produce a valid quiz after retries.");
        }
        
    //first check validation system
    private bool TryDeserializeLlmQuiz(ChatResponse inputChatResponse,out LlmQuiz response,out string error)
    {
        response = null;
        error = null;
        if (string.IsNullOrWhiteSpace(inputChatResponse.Text))
        {
            error = "LLM returned empty response";
            return false;
        }
        try
        {
         var quiz = JsonSerializer.Deserialize<LlmQuiz>(inputChatResponse.Text, new JsonSerializerOptions 
         { 
             PropertyNameCaseInsensitive = true
         });
         if (quiz is null)
         {
             error = "Deserialized quiz is null";
             return false;
         }
         response = quiz;
         return true;
        }
        catch (JsonException ex)
        {
            error = $"Invalid JSON: {ex.Message}";
            return false;
        }
    }
    //3'th check validation system
    private async Task<ChatResponse> ReturnAnswear(LlmRetryRequest retryRequest)
    {
        var message = LLMPromptSettings.BuildRepairContext(retryRequest);
        var options = new ChatOptions
        {
            ResponseFormat = ChatResponseFormat.ForJsonSchema<LlmQuiz>(
                schemaName: "quiz_schema",
                schemaDescription: "A quiz with questions and answers"
            )
        };
        var response = await clientLLama.GetResponseAsync(message, options);
        return response;
    }
    
    private List<string>  LlmValidationResult(LlmQuiz quiz)
    {
        LlmQuizValidation validation = new LlmQuizValidation();
        var resultvalidation=validation.Validate(quiz);
        var messages = resultvalidation.Errors
            .Select(e => $"{e.PropertyName}: {e.ErrorMessage}")
            .ToList();
        return messages;
    }
}
public class LlmQuizValidation : AbstractValidator<LlmQuiz>
{
    public LlmQuizValidation()
    {
        RuleFor(x => x.Title)
            .NotEmpty()
            .WithMessage("Title is required")
            .MaximumLength(1000);
        RuleFor(x => x.Tags)
            .NotNull()
            .WithMessage("Tags is required");
        RuleForEach(x => x.Tags)
            .NotEmpty()
            .NotNull()
            .MaximumLength(150)
            .WithMessage("Tag are required");
        RuleFor(x => x.Questions)
            .NotNull()
            .Must(q => q.Any()).WithMessage("At least one question is required.");
        RuleForEach(x => x.Questions)
            .SetValidator(new LlmQuestionValidation())
            .WithMessage("Questions is required");
    }
}
public class LlmQuestionValidation : AbstractValidator<LlmQuestion>
{
    public LlmQuestionValidation()
    {
        RuleFor(x => x.Explanation)
            .MaximumLength(2000)
            .When(x => !string.IsNullOrWhiteSpace(x.Explanation))
            .WithMessage("Explanation must be ≤ 2000 chars.");
        RuleFor(x => x.SourceChunkIndex)
            .GreaterThanOrEqualTo(0)
            .WithMessage("SourceChunkIndex must be >= 0.");
        RuleFor(x => x.Text)
            .Must(t => !string.IsNullOrWhiteSpace(t))
            .NotNull()
            .MaximumLength(1000)
            .WithMessage("QuestionText is required");
        RuleFor(x => x.Answers)
            .NotNull()
            .Must(a => a.Count >= 3)
            .WithMessage("At least 4 answers are required.")
            .Must(a => a.Count(ans => ans.IsCorrect) == 1)
            .WithMessage("There must be exactly one correct answer.")
            .Must(a => a.Select(ans => ans.Ordinal).Distinct().Count() == a.Count)
            .WithMessage("Answer ordinals must be unique.");
        RuleForEach(x => x.Answers)
            .SetValidator(new LlmAnswerValidation());
        
    }
}
public class LlmAnswerValidation : AbstractValidator<LlmAnswer>
{
    public LlmAnswerValidation()
    {
        //TODO: Change when we had more Answer
        RuleFor(x => x.Ordinal)
            .GreaterThanOrEqualTo(0)
            .WithMessage("Ordinal must be >= 0, Answer ordinals must be consecutive starting from 0.");;
        RuleFor(x => x.Text)
            .NotNull()
            .Must(t => !string.IsNullOrWhiteSpace(t))
            .MaximumLength(1000)
            .WithMessage("Answer is required");
    }
}