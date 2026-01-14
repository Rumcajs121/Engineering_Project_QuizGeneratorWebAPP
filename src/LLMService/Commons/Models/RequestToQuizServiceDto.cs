namespace LLMService.Commons.Models;

public record RequestQuizDto(
    Guid QuizId,
    string QuizStatus,
    Guid ExternalId,
    List<Guid> SourceId,
    string Title,
    DateTimeOffset CreatedAt,
    IEnumerable<RequestQuestionQuizDto> Question,
    IEnumerable<string> Tag
);

public record RequestQuestionQuizDto(
    Guid QuestionId,
    string Text,
    string Explanation,
    Guid SourceChunkId,
    IEnumerable<RequestAnswerQuizDto> Answer
);

public record RequestAnswerQuizDto(
    Guid AnswerId,
    int Ordinal,
    string Text,
    bool IsCorrect
);
public sealed record CreateQuizResponse(Guid Id);