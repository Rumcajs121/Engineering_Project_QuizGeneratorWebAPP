using BuildingBlocks.Exceptions;
using QuizService.Domain;
using QuizService.Domain.Abstraction;
using QuizService.Domain.IdentityValuesObject;
using QuizService.Infrastructure.Data;

namespace QuizService.Infrastructure.Repository;

public class QuizAttemptRepository(QuizDbContext db):IQuizAttemptRepository
{
    private readonly QuizDbContext _db = db;

    public Task AddAsync(QuizAttempt aggregate, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public async Task<QuizAttempt> GetAttemptQuizByIdAsync(QuizAttemptId id, CancellationToken cancellationToken = default)
    {
        var quizAttempt = await _db.QuizAttempts.AsNoTracking().Include(q => q.AttemptQuestions).ThenInclude(a => a.CorrectAnswerIds).AsSplitQuery().SingleOrDefaultAsync(q => q.Id == id, cancellationToken);
        return quizAttempt ?? throw new NotFoundException($"Quiz {id.Value} does not exist");
    }
}