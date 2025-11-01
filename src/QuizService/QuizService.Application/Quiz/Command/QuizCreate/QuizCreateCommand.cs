using BuildingBlocks.CQRS;
using FluentValidation;
using QuizService.Application.Dtos;

namespace QuizService.Application.Quiz.Command.QuizCreate;
public  record CreateQuizResult(Guid Id);

public abstract record QuizCreateCommand(CreateQuizDto CreateQuizDto) : ICommand<CreateQuizResult>;

public class QuizCreateCommandValidator : AbstractValidator<QuizCreateCommand>
{
    public QuizCreateCommandValidator()
    {
        //TODO: Validation
    }
}