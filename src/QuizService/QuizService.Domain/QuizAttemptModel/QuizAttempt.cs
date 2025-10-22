using QuizService.Domain.Abstraction;
using QuizService.Domain.Entities;
using QuizService.Domain.Exceptions;
using QuizService.Domain.IdentityValuesObject;
using QuizService.Domain.ValuesObject;

namespace QuizService.Domain;

public class QuizAttempt:Aggregate<QuizAttemptId>
{
    public QuizId QuizId { get; private set; } //reference to quiz
    public Guid UserId { get; private set; } //TODO: reference to user ??
    public string SnapchotQuizJson { get; private set; }
    public int Score { get; private set; }
    public DateTime StartQuiz { get; private set; }
    public DateTime EndTime { get; private set; }
    public int  Difficult { get; private set; }
    private readonly List<QuizAttemptQuestion> _attemptQuestions= new();
    public IReadOnlyCollection<QuizAttemptQuestion> AttemptQuestions => _attemptQuestions.AsReadOnly();

    public QuizAttempt Create(QuizId quizId,Guid userId,
        string  snapchotQuizJson,int score,
        DateTime endTim,int difficulty,IEnumerable<QuizAttemptQuestion> questions)
    {
        var qa = new QuizAttempt
        {
            Id = QuizAttemptId.Of(Guid.NewGuid()),
            QuizId = quizId,
            UserId = userId,
            SnapchotQuizJson = snapchotQuizJson,
            Score = score,
            StartQuiz = DateTime.Now,
            EndTime = endTim,
            Difficult = DifficultQuestion(difficulty)
        };
        foreach (var q in questions)
        {
            qa.AddQuestionAttempt(q);
        }
        return qa;
    }

    public void AddQuestionAttempt(QuizAttemptQuestion question)
    {
        if (question == null) throw new ArgumentNullException(nameof(question));
        if (_attemptQuestions.Any(q => q.Id == question.Id))
        {
            throw new DomainException("Question already added to this quiz.");
        }
        _attemptQuestions.Add(question);
    }

    public int DifficultQuestion(int difficulty)
    {
        if (difficulty < 1 || difficulty > 10)
            throw new DomainException("You choice difficulty from 1 to 10");
        return difficulty;
    }

    protected QuizAttempt()
    {
        
    }
}