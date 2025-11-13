using BuildingBlocks.CQRS;
using FluentValidation;
using QuizService.Application.Dtos;

namespace QuizService.Application.Quiz.Command.SubmitQuizCommand;

public record SubmitQuizCommandResult(SubmitQuizAnswerResultDto Score);

public record SubmitQuizCommand(SubmitQuizAnswersDto dto) : ICommand<SubmitQuizCommandResult>;

public class SubmitQuizCommandValidator : AbstractValidator<SubmitQuizCommand>
{
    public SubmitQuizCommandValidator()
    {
        RuleFor(x => x.dto)
            .NotNull()
            .WithMessage("Payload is required.");

        When(x => x.dto != null, () =>
        {
            RuleFor(x => x.dto.AttemptId)
                .NotEmpty()
                .WithMessage("AttemptId is required and cannot be empty Guid.");

            RuleFor(x => x.dto.Answers)
                .NotNull()
                .WithMessage("Answers collection is required.")
                .Must(a => a != null && a.Any())
                .WithMessage("At least one answer is required.");

            When(x => x.dto.Answers != null, () =>
            {
                RuleForEach(x => x.dto.Answers)
                    .ChildRules(answer =>
                    {
                        answer.RuleFor(a => a.QuizQuestionId)
                            .NotEmpty()
                            .WithMessage("QuizQuestionId is required and cannot be empty Guid.");

                        answer.RuleFor(a => a.SelectedAnswerIds)
                            .Must(selected => selected == null || selected.All(sa => sa != Guid.Empty))
                            .WithMessage("SelectedAnswerIds cannot contain empty Guid.")
                            .Must(selected => selected == null || selected.Distinct().Count() == selected.Count)
                            .WithMessage("SelectedAnswerIds must be unique per question.");
                    });
            });
        });
    }
}