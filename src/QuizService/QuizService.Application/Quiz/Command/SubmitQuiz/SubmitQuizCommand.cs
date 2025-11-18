using BuildingBlocks.CQRS;
using FluentValidation;
using QuizService.Application.Dtos;

namespace QuizService.Application.Quiz.Command.SubmitQuizCommand;

public record SubmitQuizCommandResult(int Score);

public record SubmitQuizCommand(SubmitQuizAnswersDto Dto) : ICommand<SubmitQuizCommandResult>;

public class SubmitQuizCommandValidator : AbstractValidator<SubmitQuizCommand>
{
    public SubmitQuizCommandValidator()
    {
        RuleFor(x => x.Dto)
            .NotNull()
            .WithMessage("Payload is required.");

        When(x => x.Dto != null, () =>
        {
            RuleFor(x => x.Dto.AttemptId)
                .NotEmpty()
                .WithMessage("AttemptId is required and cannot be empty Guid.");

            RuleFor(x => x.Dto.Answers)
                .NotNull()
                .WithMessage("Answers collection is required.")
                .Must(a => a != null && a.Any())
                .WithMessage("At least one answer is required.");

            When(x => x.Dto.Answers != null, () =>
            {
                RuleForEach(x => x.Dto.Answers)
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