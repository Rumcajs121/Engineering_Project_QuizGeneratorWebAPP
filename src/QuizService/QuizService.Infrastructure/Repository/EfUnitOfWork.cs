using QuizService.Domain.Abstraction;
using QuizService.Infrastructure.Data;

namespace QuizService.Infrastructure.Repository;

public class EfUnitOfWork:IUnitOfWork
{
    private readonly QuizDbContext _db;

    public EfUnitOfWork(QuizDbContext context)
    {
        _db = context;
    }
    public Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)=>_db.SaveChangesAsync(cancellationToken);
}