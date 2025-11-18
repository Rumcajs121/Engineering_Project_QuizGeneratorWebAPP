using BuildingBlocks.Exceptions;
using QuizService.Domain;
using QuizService.Domain.Abstraction;
using QuizService.Domain.Entities;
using QuizService.Domain.IdentityValuesObject;
using QuizService.Infrastructure.Data;

namespace QuizService.Infrastructure.Repository;

public class QuizAttemptRepository(QuizDbContext db):IQuizAttemptRepository
{


    public async Task AddAsync(QuizAttempt aggregate, CancellationToken cancellationToken = default)
    {
        await db.QuizAttempts.AddAsync(aggregate, cancellationToken);
    }

    public async Task<QuizAttempt> GetAttemptQuizByIdAsync(QuizAttemptId id, CancellationToken cancellationToken = default)
    {
        var quizAttempt = await db.QuizAttempts.Include(q => q.AttemptQuestions).AsSplitQuery().SingleOrDefaultAsync(q => q.Id == id, cancellationToken);
        return quizAttempt ?? throw new NotFoundException($"Quiz {id.Value} does not exist");
    }
}