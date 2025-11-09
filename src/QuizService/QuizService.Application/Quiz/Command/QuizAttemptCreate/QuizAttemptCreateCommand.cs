using BuildingBlocks.CQRS;
using QuizService.Application.Dtos;

namespace QuizService.Application.Quiz.Command.QuizAttemptCreate;


public record QuizAttemptCreateCommand(Guid QuizId) : ICommand<QuizAttemptCreateCommandResult>; 
public record QuizAttemptCreateCommandResult(QuizAttemptViewDto viewDto);