using System.Text.Json;
using FluentValidation;
using LLMService.Commons.Models;
using LLMService.Infrastructure.LLMProvider;
using LLMService.Infrastructure.VectorStore;
using Microsoft.Extensions.AI;

namespace LLMService.Infrastructure;

public interface IWorkflowGenerateQuizByLlm
{
    Task<LlmQuiz> GenerateQuizPipeline(int k, int countQuestion, string question, IReadOnlyList<Guid> documentIds,CancellationToken ct);
}
public record LlmRetryRequest(int QuestionCount, string Question, string Errors, string? IncorrectQuiz, string?  ContextRag);

public class WorkflowGenerateQuizByLlm(IChatClient clientLLama,IVectorDataRepository repository):IWorkflowGenerateQuizByLlm
{
        public async Task<LlmQuiz> GenerateQuizPipeline(int k, int countQuestion, string question, IReadOnlyList<Guid> documentIds,CancellationToken ct)
        {
            const int maxAttempts = 2;
            // First Answear.
            var contextQdrant = await repository.TopKChunk(k, question, documentIds);
            
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
                // Second Check
                if (quiz.Questions.Count != countQuestion)
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
            .MaximumLength(200);
        RuleFor(x => x.Tags)
            .NotNull()
            .WithMessage("Tags is required");
        RuleForEach(x => x.Tags)
            .NotEmpty()
            .NotNull()
            .MaximumLength(20)
            .WithMessage("Tag are required");
        RuleFor(x => x.Questions)
            .NotNull()
            .WithMessage("Questions is required");
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
            .NotEmpty()
            .NotNull()
            .MaximumLength(100)
            .WithMessage("Explanation is required");
        RuleFor(x => x.SourceChunkIndex)
            .NotEmpty()
            .NotNull()
            .GreaterThanOrEqualTo(0)
            .WithMessage("SourceChunkIndex must be greater than or equal to 0");;
        RuleFor(x => x.Text)
            .NotEmpty()
            .NotNull()
            .MaximumLength(200)
            .WithMessage("QuestionText is required");
        RuleFor(x => x.Answers)
            .Must(answers => answers.Count(a => a.IsCorrect) == 1)
            .WithMessage("There must be exactly one correct answer.");
        RuleForEach(x => x.Answers)
            .SetValidator(new LlmAnswerValidation());
        
    }
}
public class LlmAnswerValidation : AbstractValidator<LlmAnswer>
{
    public LlmAnswerValidation()
    {
        RuleFor(x => x.Ordinal)
            .InclusiveBetween(0, 3)
            .WithMessage("Ordinal must be between 0 and 3");
        RuleFor(x => x.Text)
            .NotNull()
            .NotEmpty()
            .MaximumLength(300)
            .WithMessage("Answer is required");
    }
}