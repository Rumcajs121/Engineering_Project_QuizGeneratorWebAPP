using QuizService.Application.Dtos;
using QuizService.Domain.Abstraction;
using QuizService.Infrastructure.Data;

namespace QuizService.Infrastructure.Repository;

public class QuizRepository(QuizDbContext dbContext) : IQuizRepository
{
    private readonly QuizDbContext _db = dbContext;

    public async Task AddAsync(Quiz aggregate, CancellationToken cancellationToken = default)
    {
        await _db.Quizzes.AddAsync(aggregate, cancellationToken);
    }

    public async Task<List<Quiz>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var quizzes=await _db.Quizzes.Include(x=>x.Questions).Include(x=>x.Tags).ToListAsync(cancellationToken: cancellationToken);
        return quizzes;
    }
    
}