using BuildingBlocks.CQRS;

namespace QuizService.Application.Quiz.Command.SubmitQuizCommand;
public record SubmitQuizCommandResult(bool Success);
public record SubmitQuizCommand:ICommand<SubmitQuizCommandResult>;