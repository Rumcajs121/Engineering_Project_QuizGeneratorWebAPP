using BuildingBlocks.CQRS;
using FluentValidation;
using QuizService.Application.Dtos;

namespace QuizService.Application.Quiz.Command.QuizAttemptCreate;


public record QuizAttemptCreateCommand(Guid QuizId) : ICommand<QuizAttemptCreateCommandResult>; 
public record QuizAttemptCreateCommandResult(QuizAttemptViewDto viewDto);

public class QuizAttemptCreateCommandValidator : AbstractValidator<QuizAttemptCreateCommand>
{
    public QuizAttemptCreateCommandValidator()
    {
        RuleFor(x => x.QuizId)
            .NotEmpty()
            .WithMessage("QuizId is required.");
    }
}