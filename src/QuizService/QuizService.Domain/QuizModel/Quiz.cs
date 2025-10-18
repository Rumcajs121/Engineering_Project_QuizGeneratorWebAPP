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
    
    private readonly List<Tag> _tags = new();
    public IReadOnlyCollection<Tag> Tags => _tags.AsReadOnly();
    public string? ShortDescription { get; set; }

    protected Quiz()
    {
        
    }
}