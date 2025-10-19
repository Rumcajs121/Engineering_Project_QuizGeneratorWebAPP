using QuizService.Domain.Abstraction;
using QuizService.Domain.Exceptions;
using QuizService.Domain.ValuesObject;

namespace QuizService.Domain.Models.Quiz;

public class QuizQuestion : Entity<QuizQuestionId>
{
    public string Text { get; private set; }
    public string? Explanation { get; private set; }
    public Guid? SourceChunkId { get; private set;}
    
    private readonly List<QuizAnswer> _answers = new();
    public IReadOnlyCollection<QuizAnswer> Answers => _answers.AsReadOnly();

    internal QuizQuestion(string text, string? explanation, Guid? sourceChunkId)
    {
        Id = QuizQuestionId.Of(Guid.NewGuid());
        Text = text.Trim();
        Explanation = string.IsNullOrWhiteSpace(explanation) ? null : explanation.Trim();
        SourceChunkId = (sourceChunkId.HasValue && sourceChunkId.Value == Guid.Empty) ? null : sourceChunkId;
        
    }
    public static QuizQuestion Of( string text, string? explanation, Guid? sourceChunkId,IEnumerable<(int Ordinal, string Text, bool IsCorrect)> initialAnswers)
    {
        if (string.IsNullOrWhiteSpace(text) || text.Length > 1000)
            throw new DomainException("Text is required and must be ≤ 1000 chars.");

        if (!string.IsNullOrWhiteSpace(explanation) && explanation.Length > 2000)
            throw new DomainException("Explanation must be ≤ 2000 chars.");
        
        if (sourceChunkId.HasValue && sourceChunkId.Value == Guid.Empty)
            sourceChunkId = null;
        
        var answers = (initialAnswers ?? Enumerable.Empty<(int, string, bool)>()).ToList();

        if (answers.Count != 4)
            throw new DomainException("Exactly 4 answers required.");

        if (answers.Count(a => a.IsCorrect) != 1)
            throw new DomainException("Exactly one correct answer required.");

        if (answers.Select(a => a.Ordinal).Distinct().Count() != answers.Count)
            throw new DomainException("Answer ordinals must be unique.");

        var question = new QuizQuestion(text.Trim(), explanation?.Trim(), sourceChunkId);

        foreach (var a in answers)
            question.AddAnswer(a.Ordinal, a.Text, a.IsCorrect);
        return question;
    }

    public void AddAnswer(int ordinal,string text,bool isCorrect)
    {
        if(_answers.Any(a=>a.Ordinal == ordinal)) throw new DomainException($"Answer with ordinal {ordinal} already exists for this question.");
        if (isCorrect && _answers.Any(a => a.IsCorrect))
            throw new DomainException("There is already a correct answer for this question.");
        var answer=new QuizAnswer(ordinal,text,isCorrect);
        _answers.Add(answer);
    }

    public void EditAnswer(QuizAnswerId answerId, int ordinal, string text, bool isCorrect)
    {
        var answer = _answers.FirstOrDefault(a => a.Id == answerId)
                     ?? throw new DomainException("Answer not found.");
        if (_answers.Any(a => a.Id != answerId && a.Ordinal == ordinal))
            throw new DomainException($"Answer with ordinal {ordinal} already exists for this question.");

        if (isCorrect && _answers.Any(a => a.Id != answerId && a.IsCorrect))
            throw new DomainException("There is already a correct answer for this question.");
        answer.UpdateAnswers(ordinal, text, isCorrect);
    }
    public void DeleteAnswer(QuizAnswerId answerId)
    {
        var answer = _answers.SingleOrDefault(x => x.Id == answerId)
                     ?? throw new DomainException($"Answer with id {answerId} does not exist.");
        _answers.Remove(answer);
    }
    protected QuizQuestion() { }
}
