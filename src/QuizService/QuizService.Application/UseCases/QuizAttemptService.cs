using QuizService.Application.Dtos;
using QuizService.Application.Mappers;
using QuizService.Domain;
using QuizService.Domain.Abstraction;
using QuizService.Domain.IdentityValuesObject;
using QuizService.Domain.ValuesObject;

namespace QuizService.Application.UseCases;

public class QuizAttemptService(IQuizAttemptRepository quizAttemptRepository,IQuizRepository quizRepository):IQuizAttemptService
{
    public async Task<int> SubmitQuiz(SubmitQuizAnswersDto submitDto)
    {

        var quizAttempt=await quizAttemptRepository.GetAttemptQuizByIdAsync(QuizAttemptId.Of(submitDto.AttemptId));
        var selections = submitDto.Answers.ToDictionary(
            x => QuizQuestionId.Of(x.QuizQuestionId),
            x => (IEnumerable<Guid>)((x.SelectedAnswerIds ?? []).Distinct()) 
        );
        var score = quizAttempt.SubmitAnswers(selections, DateTime.UtcNow);
        return score;
        
    }
    
    public async Task<QuizAttempt> CreateNewAnswer(Guid quizId)
    {
        var quiz=await quizRepository.GetByIdAsync(QuizId.Of(quizId));
        var snapshot=quiz.QuizToSnapshot();
        var quizAttempt=QuizAttemptMapping.ToQuizAttempt(snapshot);
        return quizAttempt;
    }
    
}