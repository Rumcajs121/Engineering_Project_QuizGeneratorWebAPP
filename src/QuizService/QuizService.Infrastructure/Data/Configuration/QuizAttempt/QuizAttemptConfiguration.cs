using System.Text.Json;
using Microsoft.EntityFrameworkCore.ChangeTracking;
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
            qa.ToTable("QuizAttemptQuestions");
            qa.Property<QuizAttemptId>("QuizAttemptId")
                .IsRequired()
                .HasColumnName("QuizAttemptId")
                .HasConversion(
                    id => id.Value,
                    value => QuizAttemptId.Of(value)
                );
            qa.HasKey("QuizAttemptId",nameof(QuizAttemptQuestion.Id));
            qa.Property(x => x.Id).HasColumnName("QuizAttemptQuestionId").ValueGeneratedNever().HasConversion(
                id => id.Value,
                value => QuizAttemptQuestionId.Of(value));
            qa.WithOwner().HasForeignKey("QuizAttemptId");
            qa.Property(x=>x.QuizQuestionId)
                .IsRequired()
                .HasColumnName("QuizQuestionId")
                .HasConversion(
                    id => id.Value,
                    value => QuizQuestionId.Of(value)
                );
            var guidListComparer = new ValueComparer<List<Guid>>(
                (a, b) => ReferenceEquals(a, b) || (a != null && b != null && a.SequenceEqual(b)),
                a => a == null ? 0 : a.Aggregate(0, (hash, g) => HashCode.Combine(hash, g.GetHashCode())),
                a => a == null ? null : a.ToList());
            
            qa.Property<List<Guid>>("_selectedAnswerIds").HasColumnName("SelectedAnswerIds").HasConversion(
                v => JsonSerializer.Serialize(v, (JsonSerializerOptions)null),
                v => JsonSerializer.Deserialize<List<Guid>>(v, (JsonSerializerOptions)null)
            )
            .HasColumnType("nvarchar(max)");
            qa.Property<List<Guid>>("_selectedAnswerIds").Metadata.SetValueComparer(guidListComparer);
            
            qa.Property<List<Guid>>("_correctAnswerIds")
                .HasColumnName("CorrectAnswerIds")
                .HasConversion(
                    v => JsonSerializer.Serialize(v, (JsonSerializerOptions)null),
                    v => JsonSerializer.Deserialize<List<Guid>>(v, (JsonSerializerOptions)null)
                )
                .HasColumnType("nvarchar(max)");
            qa.Property<List<Guid>>("_correctAnswerIds").Metadata.SetValueComparer(guidListComparer);
            qa.HasIndex("QuizQuestionId").HasDatabaseName("IX_QuizAttemptQuestions_QuizQuestionId");
            qa.Property(q => q.IsCorrect)
                .IsRequired()
                .HasColumnType("bit")
                .HasDefaultValue(false);
        });
        
    }

    private void ConfigureQuizAttemptTable(EntityTypeBuilder<Domain.QuizAttempt> builder)
    {
        builder.ToTable("QuizAttempts");
        builder.Property(e => e.Id).ValueGeneratedNever().HasColumnName("QuizAttemptId").HasConversion(
            id=>id.Value,
            value=>QuizAttemptId.Of(value));
        builder.HasKey(e => e.Id);
        builder.Property(x=>x.QuizId)
            .IsRequired()
            .HasColumnName("QuizId")
            .HasConversion(
                id => id.Value,
                value => QuizId.Of(value)
            );
        builder.Property(e => e.UserId)
            .IsRequired()
            .HasColumnName("UserId")
            .HasColumnType("uniqueidentifier");
        builder.Property(e => e.SnapshotQuizJson)
            .IsRequired()
            .HasColumnName("SnapshotQuizJson")
            .HasColumnType("nvarchar(max)");
        builder.Property(e => e.StartQuiz)
            .IsRequired()
            .HasColumnType("datetime2");
        builder.Property(e => e.SubmittedAt)
            .HasColumnType("datetime2");
        builder.Property(e => e.Difficult)
            .IsRequired()
            .HasColumnType("int")
            .HasDefaultValue(1);
        builder.HasIndex("QuizId").HasDatabaseName("IX_QuizAttempts_QuizId");
        builder.Navigation(q=>q.AttemptQuestions).Metadata.SetField("_attemptQuestions");
        builder.Navigation(q=>q.AttemptQuestions).UsePropertyAccessMode(PropertyAccessMode.Field);
    }
}