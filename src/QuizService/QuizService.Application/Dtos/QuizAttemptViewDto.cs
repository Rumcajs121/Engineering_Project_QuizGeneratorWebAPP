namespace QuizService.Application.Dtos;

public enum AnswerState
{
    Neutral=0, 
    SelectedCorrect=1, 
    SelectedWrong=2, 
    MissedCorrect=3
}

public record QuizAttemptViewDto(
    Guid QuizId, 
    string Title, 
    int Score, 
    int Difficult,
    DateTime StartQuiz, 
    DateTime EndTime, 
    TimeSpan Duration, 
    List<QuizAttemptQuestionViewDto> Questions);

public record QuizAttemptQuestionViewDto(
    string Text, 
    string? Explanation, 
    bool IsCorrect,
    List<QuizAttemptAnswerViewDto> Answers);

public record QuizAttemptAnswerViewDto(
    string Text, 
    bool IsCorrect, 
    bool SelectedByUser, 
    AnswerState State);