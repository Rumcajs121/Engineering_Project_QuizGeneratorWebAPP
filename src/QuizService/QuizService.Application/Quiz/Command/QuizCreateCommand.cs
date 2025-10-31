
using BuildingBlocks.CQRS;
using FluentValidation;
using QuizService.Application.Dtos;
using QuizService.Domain.Models.Quiz.Snapshots;
using ICommand = BuildingBlocks.CQRS.ICommand;

namespace QuizService.Application.Quiz.Command;
public  record CreateQuizResult(Guid Id);

public abstract record QuizCreateCommand(CreateQuizDto CreateQuizDto) : ICommand<CreateQuizResult>;

public class QuizCreateCommandValidator : AbstractValidator<QuizCreateCommand>
{
    public QuizCreateCommandValidator()
    {
        //TODO: Validation
    }
}