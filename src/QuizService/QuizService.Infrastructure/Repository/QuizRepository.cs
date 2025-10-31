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
}