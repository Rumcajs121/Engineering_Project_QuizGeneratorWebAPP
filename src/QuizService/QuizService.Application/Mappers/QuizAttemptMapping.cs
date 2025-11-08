using System.Text.Json;
using QuizService.Application.Dtos;
using QuizService.Domain;
using QuizService.Domain.Models.Quiz.Snapshots;

namespace QuizService.Application.Mappers;

public static class QuizAttemptMapping
{
    public static QuizAttemptViewDto QuizAttemptToDto(QuizAttempt quizAttempt)
    {
        var snapshot=JsonSerializer.Deserialize<QuizSnapshot>(quizAttempt.SnapshotQuizJson);
        var attemptQuestionsById = quizAttempt.AttemptQuestions
            .ToDictionary(aq => aq.QuizQuestionId.Value);

        var questionDtos = snapshot.Questions.Select(q =>
        {
            var aq = attemptQuestionsById[q.Id];

            var selected = aq.SelectedAnswerIds.ToHashSet();
            var correct = q.Answers.Where(a => a.IsCorrect)
                .Select(a => a.Id)
                .ToHashSet();

            bool isQuestionCorrect = selected.SetEquals(correct);
            
            var answerDtos = q.Answers.Select(a =>
            {
                bool selectedByUser = selected.Contains(a.Id);
                var state =
                    selectedByUser && a.IsCorrect ? AnswerState.SelectedCorrect :
                    selectedByUser && !a.IsCorrect ? AnswerState.SelectedWrong :
                    !selectedByUser && a.IsCorrect ? AnswerState.MissedCorrect :
                    AnswerState.Neutral;

                return new QuizAttemptAnswerViewDto(
                    Text: a.Text,
                    IsCorrect: a.IsCorrect,
                    SelectedByUser: selectedByUser,
                    State: state
                );
            }).ToList();
            return new QuizAttemptQuestionViewDto(
                Text: q.Text,
                Explanation: q.Explanation,
                IsCorrect: isQuestionCorrect,
                Answers: answerDtos
            );
        })
            .ToList();
        return new QuizAttemptViewDto(
            QuizId: snapshot.QuizId,
            Score: quizAttempt.Score,
            Title: snapshot.QuizName,
            Difficult: quizAttempt.Difficult,
            StartQuiz: quizAttempt.StartQuiz,
            EndTime: quizAttempt.SubmittedAt,
            Questions: questionDtos
        );
    }
    
}