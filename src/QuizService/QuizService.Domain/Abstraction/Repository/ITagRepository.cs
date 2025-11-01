using QuizService.Domain.ValuesObject;

namespace QuizService.Domain.Abstraction.Repository;

public interface ITagRepository
{
    Task<List<Tag>> GetByNameAsync(IEnumerable<string> tagNames,CancellationToken cancellationToken = default);
    void AddRange(IEnumerable<Tag> tags);
}