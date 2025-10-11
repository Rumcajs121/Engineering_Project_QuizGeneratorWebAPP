using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace QuizService.Infrastructure.Data.Configuration;

public class QuizConfiguration:IEntityTypeConfiguration<Quiz>
{
    public void Configure(EntityTypeBuilder<Quiz> builder)
    {
        builder.HasKey(o=>o.Id);
        builder.Property(o=>o.Id).HasConversion(
            quizId => quizId.Value,
            dbId=>QuizId.Of(dbId)
            ).ValueGeneratedNever();
        builder.HasMany(o=>o.Questions)
            .WithOne()
            .HasForeignKey(o=>o.QuizId)
            .IsRequired()
            .OnDelete(DeleteBehavior.Cascade);
        
    }
}