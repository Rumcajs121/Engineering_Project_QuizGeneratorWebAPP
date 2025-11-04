namespace QuizService.Domain.Exceptions;

//TODO: Delete ?? 
public class DomainException(string message) : Exception($"Domain Exception: \"{message}\" throws from Domain Layer");
