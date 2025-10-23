using Microsoft.EntityFrameworkCore.Metadata.Builders;
using QuizService.Domain.Entities;
using QuizService.Domain.IdentityValuesObject;

namespace QuizService.Infrastructure.Data.Configuration.QuizAttempt;

public class QuizAttemptConfiguration:IEntityTypeConfiguration<Domain.QuizAttempt>
{
    public void Configure(EntityTypeBuilder<Domain.QuizAttempt> builder)
    {
        ConfigureQuizAttemptTable(builder);
        ConfigureQuizAttemptQuestionTable(builder);
    }

    private void ConfigureQuizAttemptQuestionTable(EntityTypeBuilder<Domain.QuizAttempt> builder)
    {
        builder.OwnsMany<QuizAttemptQuestion>(q => q.AttemptQuestions, qa =>
        {
            qa.ToTable("QuizzesAttemptQuestions");
            qa.HasKey("QuizAttemptId",nameof(QuizAttemptQuestion.Id));
            qa.Property(x => x.Id).HasColumnName("QuizAttemptQuestionId").ValueGeneratedNever().HasConversion(
                id => id.Value,
                value => QuizAttemptQuestionId.Of(Guid.NewGuid()));
            qa.WithOwner().HasForeignKey("QuizId");
            //ToDO: Will secure exceptions other property
            
        });
    }

    private void ConfigureQuizAttemptTable(EntityTypeBuilder<Domain.QuizAttempt> builder)
    {
        builder.ToTable("QuizzesAttempts");
        builder.Property(e => e.Id).ValueGeneratedNever().HasColumnName("QuizAttemptId").HasConversion(
            id=>id.Value,
            value=>QuizAttemptId.Of(Guid.NewGuid()));
        builder.HasKey(e => e.Id);
        //TODO: Security property for exceptions
        builder.Navigation(q=>q.AttemptQuestions).Metadata.SetField("_attemptQuestions");
        builder.Navigation(q=>q.AttemptQuestions).UsePropertyAccessMode(PropertyAccessMode.Field);
    }
}