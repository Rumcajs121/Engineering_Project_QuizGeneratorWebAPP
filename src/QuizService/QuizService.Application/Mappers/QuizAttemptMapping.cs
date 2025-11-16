using Newtonsoft.Json;
using QuizService.Application.Dtos;
using QuizService.Domain;
using QuizService.Domain.Entities;
using QuizService.Domain.Exceptions;
using QuizService.Domain.IdentityValuesObject;
using QuizService.Domain.Models.Quiz.Snapshots;
using QuizService.Domain.ValuesObject;

namespace QuizService.Application.Mappers;

public static class QuizAttemptMapping
{
    public static QuizAttemptViewDto QuizAttemptToDto(QuizAttempt attempt)
    {
        var snapshot = JsonConvert.DeserializeObject<QuizSnapshot>(attempt.SnapshotQuizJson)
                       ?? throw new DomainException("Invalid quiz snapshot JSON.");

        var questionsDto = snapshot.Questions
            .Select(snapshotQuestion =>
            {
                var attemptQuestion = attempt.AttemptQuestions
                    .FirstOrDefault(aq => aq.QuizQuestionId.Value == snapshotQuestion.Id);

                bool isCorrect = attemptQuestion?.IsCorrect ?? false;

                var answersDto = snapshotQuestion.Answers
                    .Select(a => new QuizAttemptAnswerViewDto(
                        AnswerId: a.Id,
                        Text: a.Text
                    ))
                    .ToList();

                return new QuizAttemptQuestionViewDto(
                    QuestionId: snapshotQuestion.Id,
                    Text: snapshotQuestion.Text,
                    Explanation: snapshotQuestion.Explanation,
                    IsCorrect: isCorrect,
                    Answers: answersDto
                );
            })
            .ToList();

        return new QuizAttemptViewDto(
            QuizId: snapshot.QuizId.Value,
            Title: snapshot.QuizName,
            Score: attempt.Score,
            Difficult: attempt.Difficult ?? 1,
            StartQuiz: attempt.StartQuiz,
            SubmittedAt: attempt.SubmittedAt,
            Questions: questionsDto
        );
    }

    public static QuizAttempt ToQuizAttempt(QuizSnapshot quiz)
    {
        var snapshotToJson = JsonConvert.SerializeObject(quiz);
        var questionAttempt = new List<QuizAttemptQuestion>();
        foreach (var q in quiz.Questions)
        {
            var correctIds = q.Answers
                .Where(a => a.IsCorrect)
                .Select(a => a.Id)
                .ToList();
            var attemptQ = QuizAttemptQuestion.Of(
                quizAttemptQuestionId:QuizAttemptQuestionId.Of(Guid.NewGuid()), 
                quizQuestionId: QuizQuestionId.Of(q.Id),
                correctAnswerIds: correctIds
            );
            questionAttempt.Add(attemptQ);
        }
        return QuizAttempt.Create(quiz.QuizId,Guid.NewGuid(),snapshotToJson,DateTime.Now, 1,questionAttempt);
    }
    
}