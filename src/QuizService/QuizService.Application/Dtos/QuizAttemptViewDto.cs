namespace QuizService.Application.Dtos;


public record QuizAttemptViewDto(
    Guid QuizId,
    string Title,
    int Score,
    int Difficult,
    DateTime StartQuiz,
    DateTime? SubmittedAt,
    List<QuizAttemptQuestionViewDto> Questions);

public record QuizAttemptQuestionViewDto(
    Guid QuestionId,
    string Text,
    string? Explanation,
    bool IsCorrect, 
    List<QuizAttemptAnswerViewDto> Answers);
public record QuizAttemptAnswerViewDto(
    Guid AnswerId,
    string Text);