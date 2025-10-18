

using QuizService.Domain.Abstraction;

namespace QuizService.Infrastructure.Data;

public class QuizDbContext(DbContextOptions<QuizDbContext> options):DbContext(options)
{
    public DbSet<Quiz> Quizzes => Set<Quiz>();
    public DbSet<Tag>  Tags => Set<Tag>();

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.Ignore<DomainEvent>();
        builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        base.OnModelCreating(builder);
    }
}