using System.Data;
using QuizService.Domain.Abstraction;
using QuizService.Domain.Enums;
using QuizService.Domain.Events;
using QuizService.Domain.Exceptions;
using QuizService.Domain.ValuesObject;

namespace QuizService.Domain.Models.Quiz;

public class Quiz:Aggregate<QuizId>
{
    public QuizStatus QuizStatus { get; set; }
    public Guid SourceId { get; set; }
    private readonly List<QuizQuestion> _questions = new();
    public IReadOnlyCollection<QuizQuestion> Questions => _questions.AsReadOnly();
    public string? ShortDescription { get; set; }

    public static Quiz Create(QuizId id, QuizStatus quizStatus, Guid sourceId, string shortDescription, IEnumerable<QuizQuestion> questions)
    {
        var quiz = new Quiz
        {
            Id = id,
            QuizStatus = QuizStatus.Generating,
            SourceId = sourceId,
            ShortDescription = shortDescription,
        };
        foreach (var question in questions)
        {
            quiz._questions.Add(question);
        }
        quiz.AddDomainEvent(new QuizGenerateEvent(quiz));
        return quiz;
    }

    public  void Archive()
    {
        if (QuizStatus == QuizStatus.Archived)
        {
            return;
        }
        if (QuizStatus == QuizStatus.Failed || QuizStatus == QuizStatus.Generating)
        {
            throw new DomainException("Quiz isn't already archived");
        }
        QuizStatus = QuizStatus.Archived;
    }
}