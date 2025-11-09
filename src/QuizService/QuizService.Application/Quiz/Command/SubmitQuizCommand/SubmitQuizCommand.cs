using BuildingBlocks.CQRS;
using QuizService.Application.Dtos;

namespace QuizService.Application.Quiz.Command.SubmitQuizCommand;
public record SubmitQuizCommandResult(SubmitQuizAnswerResultDto Score);
public record SubmitQuizCommand(SubmitQuizAnswersDto dto):ICommand<SubmitQuizCommandResult>;