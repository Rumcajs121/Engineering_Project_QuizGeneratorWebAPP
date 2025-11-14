using BuildingBlocks.CQRS;
using FluentValidation;
using QuizService.Application.Dtos;

namespace QuizService.Application.Quiz.Query.GetQuizById;

public record GetQuizByIdResult(QuizDto QuizDto);
public record GetQuizByIdQuery(Guid QuizId):IQuery<GetQuizByIdResult>;

public class GetQuizByIdQueryValidator : AbstractValidator<GetQuizByIdQuery>
{
    public GetQuizByIdQueryValidator ()
    {
        RuleFor(x => x.QuizId)
            .NotEmpty()
            .WithMessage("QuizId is required and cannot be empty Guid.");
    }
}