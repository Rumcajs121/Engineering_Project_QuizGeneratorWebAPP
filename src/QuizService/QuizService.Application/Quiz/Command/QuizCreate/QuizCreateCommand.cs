using BuildingBlocks.CQRS;
using FluentValidation;
using QuizService.Application.Dtos;
using QuizService.Domain.Enums;

namespace QuizService.Application.Quiz.Command.QuizCreate;

public record CreateQuizResult(Guid Id);

public record QuizCreateCommand(CreateQuizDto CreateQuizDto) : ICommand<CreateQuizResult>;

public class QuizCreateCommandValidator : AbstractValidator<QuizCreateCommand>
{
    public QuizCreateCommandValidator()
    {
        RuleFor(x => x.CreateQuizDto).NotNull();

        When(x => x.CreateQuizDto != null, () =>
        {
            RuleFor(x => x.CreateQuizDto.SourceId)
                .NotNull()
                .Must(ids => ids!.Count > 0);

            RuleFor(x => x.CreateQuizDto.ExternalId)
                .NotEmpty();

            RuleFor(x => x.CreateQuizDto.Question)
                .NotNull()
                .Must(q => q!.Any());
        });
    }
}

