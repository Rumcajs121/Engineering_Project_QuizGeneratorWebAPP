using QuizService.Application.Dtos;
using QuizService.Domain.Abstraction;
using QuizService.Domain.IdentityValuesObject;
using QuizService.Domain.ValuesObject;

namespace QuizService.Application.UseCases;

public class QuizAttemptService(IQuizAttemptRepository _repository):IQuizAttemptService
{
    public async Task<int> SubmitQuiz(SubmitQuizAnswersDto submitDto)
    {

        var quizAttempt=await _repository.GetAttemptQuizByIdAsync(QuizAttemptId.Of(submitDto.AttemptId));
        var selections = submitDto.Answers.ToDictionary(
            x => QuizQuestionId.Of(x.QuizQuestionId),
            x => (IEnumerable<Guid>)((x.SelectedAnswerIds ?? []).Distinct()) 
        );
        var score = quizAttempt.SubmitAnswers(selections, DateTime.UtcNow);
        
        //TODO: Zapis do bazy !!
        // await _repository.UpdateAsync(quizAttempt);
        // await _repository.SaveChangesAsync();
        return score;
    }
}