
namespace QuizService.Application.Dtos;

public record SubmitQuizAnswersDto(
    Guid AttemptId,
    IReadOnlyList<SubmitAnswerDto> Answers
);

public record SubmitAnswerDto(
    Guid QuizQuestionId,
    IReadOnlyList<Guid> SelectedAnswerIds
);