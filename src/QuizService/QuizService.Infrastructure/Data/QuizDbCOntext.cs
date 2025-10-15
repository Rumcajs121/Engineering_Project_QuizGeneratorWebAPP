

namespace QuizService.Infrastructure.Data;

public class QuizDbCOntext(DbContextOptions<QuizDbCOntext> options):DbContext(options)
{
    public DbSet<Quiz> Quizzes => Set<Quiz>();

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        base.OnModelCreating(builder);
    }
}