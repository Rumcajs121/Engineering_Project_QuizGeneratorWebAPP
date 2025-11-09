using QuizService.Application.Dtos;
using QuizService.Application.Mappers;
using QuizService.Domain;
using QuizService.Domain.Abstraction;
using QuizService.Domain.IdentityValuesObject;
using QuizService.Domain.ValuesObject;

namespace QuizService.Application.UseCases;

public class QuizAttemptService(IQuizAttemptRepository _quizAttemptRepository,IQuizRepository _quizRepository):IQuizAttemptService
{
    public async Task<SubmitQuizAnswerResultDto> SubmitQuiz(SubmitQuizAnswersDto submitDto)
    {

        var quizAttempt=await _quizAttemptRepository.GetAttemptQuizByIdAsync(QuizAttemptId.Of(submitDto.AttemptId));
        var selections = submitDto.Answers.ToDictionary(
            x => QuizQuestionId.Of(x.QuizQuestionId),
            x => (IEnumerable<Guid>)((x.SelectedAnswerIds ?? []).Distinct()) 
        );
        var score = quizAttempt.SubmitAnswers(selections, DateTime.UtcNow);
        var maxScore=quizAttempt.AttemptQuestions.Count();
        
        return new SubmitQuizAnswerResultDto(score,maxScore);
        
    }

    public async Task<QuizAttempt> CreateNewAnswer(Guid orderId)
    {
        var quiz=await _quizRepository.GetByIdAsync(QuizId.Of(orderId));
        var snapshot=quiz.QuizToSnapshot();
        var quizAttempt=QuizAttemptMapping.ToQuizAttempt(snapshot);
        return quizAttempt;
    }
}