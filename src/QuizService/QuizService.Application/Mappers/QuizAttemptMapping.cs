using System.Text.Json;
using Microsoft.AspNetCore.SignalR;
using QuizService.Application.Dtos;
using QuizService.Domain;
using QuizService.Domain.Entities;
using QuizService.Domain.IdentityValuesObject;
using QuizService.Domain.Models.Quiz.Snapshots;
using QuizService.Domain.ValuesObject;

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
            QuizId: snapshot.QuizId.Value,
            Score: quizAttempt.Score,
            Title: snapshot.QuizName,
            Difficult: quizAttempt.Difficult ?? 1,
            StartQuiz: quizAttempt.StartQuiz,
            EndTime: quizAttempt.SubmittedAt,
            Questions: questionDtos
        );
    }

    public static QuizAttempt ToQuizAttempt(QuizSnapshot quiz)
    {
        var snapshotToJson = JsonSerializer.Serialize(quiz);
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