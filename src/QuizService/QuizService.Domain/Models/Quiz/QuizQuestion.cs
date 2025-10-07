using QuizService.Domain.Abstraction;
using QuizService.Domain.ValuesObject;

namespace QuizService.Domain.Models.Quiz;

public class QuizQuestion : Entity<QuizQuestionId>
{
    public QuizId QuizId { get; }
    public string Text { get; }
    public string? Explanation { get; }
    public Guid? SourceChunkId { get; }
    private readonly List<QuizAnswer> _answers = new();
    public IReadOnlyCollection<QuizAnswer> Answers => _answers.AsReadOnly();
    private readonly List<QuizTag> _tags = new();
    public IReadOnlyCollection<QuizTag> Tags => _tags.AsReadOnly();
}


// TODO: Design domain method 
//     protected QuizQuestion()
//     {
//         
//     }
//
//     private QuizQuestion(
//         string text,
//         string? explanation,
//         IEnumerable<(string Text, bool IsCorrect)> answers,IEnumerable<QuizTag> tags,Guid? sourceChunkId,QuizId quizId)
//     {
//         ArgumentNullException.ThrowIfNullOrWhiteSpace(text);
//         if (answers == null || !answers.Any())
//             throw new ArgumentException("Question must have at least one answer.", nameof(answers));
//
//         Text = text;
//         Explanation = explanation;
//
//         int ordinal = 0;
//         foreach (var (answerText, isCorrect) in answers)
//         {
//             var answer = QuizAnswer.Of(
//                 ordinal: ordinal++,
//                 text: answerText,
//                 isCorrect: isCorrect,
//                 question: this
//             );
//             _answers.Add(answer);
//         }
//         foreach (var tag in tags)
//         {
//             _tags.Add(tag);
//         }
//         SourceChunkId= sourceChunkId;
//         if (quizId is null)
//             throw new ArgumentNullException(nameof(quizId));
//         QuizId = quizId;
//     }
//     public static QuizQuestion Of(
//         string text,
//         string? explanation,
//         IEnumerable<(string Text, bool IsCorrect)> answers,IEnumerable<QuizTag> tags,Guid? sourceChunkId, QuizId quizId)
//     {
//         return new QuizQuestion(text, explanation, answers, tags,sourceChunkId,quizId);
//     }
// }
