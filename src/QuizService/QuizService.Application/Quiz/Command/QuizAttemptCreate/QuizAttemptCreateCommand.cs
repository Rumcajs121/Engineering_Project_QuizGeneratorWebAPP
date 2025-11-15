using BuildingBlocks.CQRS;
using FluentValidation;
using QuizService.Application.Dtos;

namespace QuizService.Application.Quiz.Command.QuizAttemptCreate;

public record QuizAttemptCreateCommandResult(QuizAttemptViewDto ViewDto);

public record QuizAttemptCreateCommand(Guid QuizId) : ICommand<QuizAttemptCreateCommandResult>; 

public class QuizAttemptCreateCommandValidator : AbstractValidator<QuizAttemptCreateCommand>
{
    public QuizAttemptCreateCommandValidator()
    {
        RuleFor(x => x.QuizId)
            .NotEmpty()
            .WithMessage("QuizId is required.");
    }
}