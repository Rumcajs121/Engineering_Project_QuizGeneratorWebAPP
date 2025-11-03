using BuildingBlocks.CQRS;
using FluentValidation;
using QuizService.Application.Dtos;

namespace QuizService.Application.Quiz.Command.QuizCreate;
public  record CreateQuizResult(Guid Id);

public record QuizCreateCommand(CreateQuizDto CreateQuizDto) : ICommand<CreateQuizResult>;



public class QuizCreateCommandValidator : AbstractValidator<QuizCreateCommand>
{
    public QuizCreateCommandValidator()
    {
        RuleForEach(x => x.CreateQuizDto.Question).ChildRules(q =>
        {
            q.RuleFor(x => x.Answer).NotNull().Must(a => a.Count() == 4)
                .WithMessage("Exactly 4 answers required.");
            q.RuleFor(x => x.Answer.Count(a => a.IsCorrect)).Equal(1)
                .WithMessage("Exactly 1 correct answer required.");
        });
        //TODO: Validation
    }
}