using QuizService.Domain.Abstraction;
using QuizService.Domain.Abstraction.Repository;
using QuizService.Infrastructure.Data;

namespace QuizService.Infrastructure.Repository;

public class TagRepository(QuizDbContext dbContext):ITagRepository
{
    private readonly QuizDbContext _dbContext = dbContext;

    public async Task<List<Tag>> GetByNameAsync(IEnumerable<string> tagNames, CancellationToken cancellationToken = default)
    {
        var normalized = tagNames
            .Where(n => !string.IsNullOrWhiteSpace(n))
            .Select(n => n.Trim())
            .Distinct(StringComparer.OrdinalIgnoreCase)
            .ToList();
        return await _dbContext.Tags.Where(t => normalized.Contains(t.Name)).ToListAsync(cancellationToken); 
    }

    public void AddRange(IEnumerable<Tag> tags)
    {
        _dbContext.Tags.AddRange(tags);
    }
}