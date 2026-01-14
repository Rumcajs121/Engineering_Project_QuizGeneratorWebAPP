using QuizService.Application.Dtos;
using QuizService.Domain.Abstraction.Repository;
using QuizService.Domain.Models.Quiz;
using QuizService.Domain.ValuesObject;

namespace QuizService.Application.UseCases;

public class QuizService(ITagRepository tagRepository):IQuizService
{
    public async Task<Domain.Models.Quiz.Quiz> CreateNewQuiz(CreateQuizDto dto)
    {
        var listNewQuestion=new List<QuizQuestion>();
        foreach (var qDto in dto.Question)
        {
            var answersTuples = qDto.Answer.Select(a => (a.Ordinal, a.Text, a.IsCorrect));
            var newQuestion = QuizQuestion.Of(
                text: qDto.Text,
                explanation: qDto.Explanation,
                sourceChunkId: qDto.SourceChunkId,
                initialAnswers: answersTuples
            );
            listNewQuestion.Add(newQuestion);
        }

        
        var newQuiz = Domain.Models.Quiz.Quiz.Create(
            sourceId: dto.SourceId,
            externalId:dto.ExternalId,
            titleQuiz: dto.Title,
            questions: listNewQuestion
        );
        var tags = await CreateTag(dto.Tag);
        
        foreach (var tag in tags)
            newQuiz.AddTag(tag);
        
        return newQuiz;
    }
    
    //TODO: Check ?? 
    private async Task<IEnumerable<Tag>> CreateTag(IEnumerable<string> tagName)
    {
        IEnumerable<Tag> allTags = [];
        var existing = await tagRepository.GetByNameAsync(tagName);
        var existingNames = existing
            .Select(t => t.Name)
            .ToHashSet(StringComparer.OrdinalIgnoreCase);
        var missing = tagName
            .Where(n => !existingNames.Contains(n))
            .Select(Tag.Of)
            .ToList();
        if (missing.Count > 0)
        {
            tagRepository.AddRange(missing);
        }
        allTags = existing.Concat(missing).ToList();
        return allTags;
    }
}
