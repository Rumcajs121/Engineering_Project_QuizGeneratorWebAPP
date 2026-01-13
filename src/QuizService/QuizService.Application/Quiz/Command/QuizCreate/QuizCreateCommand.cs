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
        RuleFor(x => x.CreateQuizDto)
            .NotNull()
            .WithMessage("CreateQuizDto is required.");

        When(x => x.CreateQuizDto != null, () =>
        {
            //TODO: Change Validator
            // RuleFor(x => x.CreateQuizDto.SourceId)
            //     .NotEmpty()
            //     .WithMessage("SourceId is required and cannot be empty Guid.");

            RuleFor(x => x.CreateQuizDto.Title)
                .MaximumLength(500)
                .WithMessage("Title must be ≤ 500 chars.");
            RuleFor(x => x.CreateQuizDto.Question)
                .NotNull()
                .WithMessage("Questions collection is required.")
                .Must(q => q != null && q.Any())
                .WithMessage("Quiz must contain at least one question.");

            RuleFor(x => x.CreateQuizDto.Tag)
                .Must(tags => tags == null
                              || tags.All(t => !string.IsNullOrWhiteSpace(t)
                                               && t.Trim().Length <= 50))
                .WithMessage("Tags must be non-empty and ≤ 50 chars.")
                .Must(tags => tags == null
                              || tags.Select(t => t.Trim())
                                  .Distinct(StringComparer.OrdinalIgnoreCase)
                                  .Count() == tags.Count())
                .WithMessage("Tag names must be unique.");

            When(x => x.CreateQuizDto.Question != null, () =>
            {
                RuleForEach(x => x.CreateQuizDto.Question)
                    .ChildRules(question =>
                    {
                        question.RuleFor(q => q.QuestionId)
                            .NotEmpty()
                            .WithMessage("QuestionId is required and cannot be empty Guid.");

                        question.RuleFor(q => q.Text)
                            .NotEmpty()
                            .WithMessage("Question Text is required.")
                            .MaximumLength(1000)
                            .WithMessage("Question Text must be ≤ 1000 chars.");

                        question.RuleFor(q => q.Explanation)
                            .MaximumLength(2000)
                            .When(q => !string.IsNullOrWhiteSpace(q.Explanation))
                            .WithMessage("Question Explanation must be ≤ 2000 chars.");

                        question.RuleFor(q => q.Answer)
                            .NotNull()
                            .WithMessage("Answers collection is required.")
                            .Must(answers => answers != null && answers.Count() == 4)
                            .WithMessage("Exactly 4 answers required per question.")
                            .DependentRules(() =>
                            {
                                question.RuleFor(q => q.Answer)
                                    .Must(answers => answers.Count(a => a.IsCorrect) == 1)
                                    .WithMessage("Exactly 1 correct answer required per question.");

                                question.RuleFor(q => q.Answer)
                                    .Must(answers => answers.All(a => a.Ordinal >= 0))
                                    .WithMessage("Answer ordinals must be >= 0.")
                                    .Must(answers =>
                                        answers.Select(a => a.Ordinal).Distinct().Count() == answers.Count())
                                    .WithMessage("Answer ordinals must be unique within a question.");

                                question.RuleForEach(q => q.Answer)
                                    .ChildRules(answer =>
                                    {
                                        answer.RuleFor(a => a.AnswerId)
                                            .NotEmpty()
                                            .WithMessage("AnswerId is required and cannot be empty Guid.");

                                        answer.RuleFor(a => a.Text)
                                            .NotEmpty()
                                            .WithMessage("Answer Text is required and cannot be blank.")
                                            .MaximumLength(1000)
                                            .WithMessage("Answer Text must be ≤ 1000 chars.");

                                        answer.RuleFor(a => a.Ordinal)
                                            .GreaterThanOrEqualTo(0)
                                            .WithMessage("Answer Ordinal must be >= 0.");
                                    });
                            });
                    });
            });
        });
    }
}