using QuizService.Application.Dtos;
using QuizService.Domain;

namespace QuizService.Application.Mappers;

public static class QuizAttemptMapping
{
    public static QuizDto QuizAttemptToDto(QuizAttempt quizAttempt)
    {
        var quizDto = new QuizDto()
     {
         //TODO: Mapping to attemptDTO ?? 
         // QuestionDtos = quizAttempt.AttemptQuestions.Select(
         //     q=>new QuizQuestionDto
         //     {
         //         // Text = ,
         //         Explanation = q.Explanation,
         //         SourceChunkId = q.SourceChunkId,
         //         Answers = q.Answers.Select(a=>new QuizAnswerDto
         //         {
         //             Ordinal = a.Ordinal,
         //             Text = a.Text,
         //             IsCorrect = a.IsCorrect
         //         }).ToList()
         //     }).ToList()
     };
        return quizDto;
    }
}

// public static QuizDto ToQuizDto(Domain.Models.Quiz.Quiz quiz)
// {
//     var quizDto = new QuizDto()
//     {
//         QuestionDtos = quiz.Questions.Select(
//             q=>new QuizQuestionDto
//             {
//                 Text = q.Text,
//                 Explanation = q.Explanation,
//                 SourceChunkId = q.SourceChunkId,
//                 Answers = q.Answers.Select(a=>new QuizAnswerDto
//                 {
//                     Ordinal = a.Ordinal,
//                     Text = a.Text,
//                     IsCorrect = a.IsCorrect
//                 }).ToList()
//             }).ToList()
//     };
//     return quizDto;
// }