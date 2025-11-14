using BuildingBlocks.CQRS;
using FluentValidation;
using QuizService.Application.Dtos;

namespace QuizService.Application.Quiz.Query.GetAttemptQuizById;

public record GetAttemptQuizByIdResult(QuizAttemptViewDto AttemptByUser);
public record GetAttemptQuizByIdQuery(Guid QuizAttemptId):IQuery<GetAttemptQuizByIdResult>;

public class GetAttemptQuizByIdValidator : AbstractValidator<GetAttemptQuizByIdQuery>
{
    public GetAttemptQuizByIdValidator()
    {
        RuleFor(x => x.QuizAttemptId)
            .NotEmpty()
            .WithMessage("QuizAttemptId is required and cannot be empty Guid.");
    }
}