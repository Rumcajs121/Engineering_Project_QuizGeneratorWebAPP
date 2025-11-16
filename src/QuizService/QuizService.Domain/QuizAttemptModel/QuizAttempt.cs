using QuizService.Domain.Abstraction;
using QuizService.Domain.Entities;
using QuizService.Domain.Exceptions;
using QuizService.Domain.IdentityValuesObject;
using QuizService.Domain.ValuesObject;

namespace QuizService.Domain;

public class QuizAttempt:Aggregate<QuizAttemptId>
{
    public QuizId QuizId { get; private set; }
    public Guid? UserId { get; private set; } //TODO: reference to user ??
    public string SnapshotQuizJson { get; private set; }
    public int Score { get; private set; }
    public DateTime StartQuiz { get; private set; }
    public DateTime? SubmittedAt { get; private set; }
    public int?  Difficult { get; private set; }
    private readonly List<QuizAttemptQuestion> _attemptQuestions= new();
    public IReadOnlyCollection<QuizAttemptQuestion> AttemptQuestions => _attemptQuestions.AsReadOnly();

    public static QuizAttempt Create(
        QuizId quizId,
        Guid userId,
        string snapshotQuizJson,
        DateTime startTime,
        int difficulty,
        IEnumerable<QuizAttemptQuestion> questions)
    {
        if (questions is null || !questions.Any())
            throw new DomainException("QuizAttempt must contain questions.");

        var qa = new QuizAttempt
        {
            Id = QuizAttemptId.Of(Guid.NewGuid()),
            QuizId = quizId,
            UserId = userId,
            SnapshotQuizJson = snapshotQuizJson,
            Score = 0,
            StartQuiz = startTime,
            Difficult = ValidateDifficulty(difficulty)
        };
        foreach (var q in questions) 
            qa.AddQuestionAttempt(q);
        return qa;
    }

    public void AddQuestionAttempt(QuizAttemptQuestion question)
    {
        if (question == null) throw new ArgumentNullException(nameof(question));
        if (_attemptQuestions.Any(q => q.QuizQuestionId == question.QuizQuestionId)) //TODO: Secure duplicate ?? 
            throw new DomainException("Question already added to this quiz.");
        _attemptQuestions.Add(question);
    }

    public static int ValidateDifficulty(int difficulty)
    {
        if (difficulty < 1 || difficulty > 10)
            throw new DomainException("You choice difficulty from 1 to 10");
        return difficulty;
    }
    public int SubmitAnswers(
        IReadOnlyDictionary<QuizQuestionId, IEnumerable<Guid>> selections,
        DateTime submittedAtUtc)
    {
        if (SubmittedAt is not null)
            throw new DomainException("Quiz already submitted.");

        foreach (var q in _attemptQuestions)
        {
            var selected = selections.TryGetValue(q.QuizQuestionId, out var ids)
                ? ids
                : Enumerable.Empty<Guid>();

            q.ApplySelection(selected);
        }

        Score = _attemptQuestions.Count(q => q.IsCorrect);
        SubmittedAt = submittedAtUtc;
        return Score;
    }

    protected QuizAttempt()
    {
        
    }
}