using QuizService.Domain.Abstraction;
using QuizService.Domain.Models.Quiz;

namespace QuizService.Domain.Events;

public record QuizGenerateEvent(Quiz quiz):DomainEvent;