namespace QuizService.Domain.Exceptions;

public class NotFoundException(string message):Exception($"NotFound Exception: {message} ");