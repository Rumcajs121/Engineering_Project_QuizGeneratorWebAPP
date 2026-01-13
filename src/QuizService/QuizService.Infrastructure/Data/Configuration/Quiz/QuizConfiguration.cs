using System.Text.Json;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using QuizService.Domain.Enums;

namespace QuizService.Infrastructure.Data.Configuration;

public class QuizConfiguration : IEntityTypeConfiguration<Quiz>
{
    public void Configure(EntityTypeBuilder<Quiz> builder)
    {
        ConfigureQuizTable(builder);
        ConfigureQuizQuestionTable(builder);
    }
    private void ConfigureQuizQuestionTable(EntityTypeBuilder<Quiz> builder)
    {
        builder.OwnsMany<QuizQuestion>(q => q.Questions, qq =>
            {
                qq.ToTable("QuizQuestions");
                qq.Property<QuizId>("QuizId")
                    .IsRequired()
                    .HasColumnName("QuizId")
                    .HasConversion(
                        id => id.Value,
                        value => QuizId.Of(value));
                qq.HasKey("QuizId", nameof(QuizQuestion.Id));
                qq.Property(x => x.Id)
                    .HasColumnName("QuizQuestionId")
                    .ValueGeneratedNever()
                    .HasConversion(
                        id => id.Value,
                        value => QuizQuestionId.Of(value));
                qq.WithOwner()
                    .HasForeignKey("QuizId");
                
                
                qq.Property(x => x.Text)
                    .HasMaxLength(1000)
                    .IsRequired()
                    .IsUnicode(true);
                qq.Property(x => x.Explanation)
                    .HasMaxLength(2000)
                    .IsRequired(false)
                    .IsUnicode(true);
                qq.Property(x => x.SourceChunkId)
                    .IsRequired(false);

                qq.OwnsMany(x => x.Answers, qa =>
                {
                    qa.ToTable("QuizAnswers");
                    qa.Property<QuizId>("QuizId")
                        .HasColumnName("QuizId")
                        .HasConversion(
                            id => id.Value,
                            value => QuizId.Of(value));

                    qa.Property<QuizQuestionId>("QuizQuestionId")
                        .HasColumnName("QuizQuestionId")
                        .HasConversion(
                            id => id.Value,
                            value => QuizQuestionId.Of(value));
                    qa.Property(x => x.Id)
                        .HasColumnName("QuizAnswerId")
                        .ValueGeneratedNever()
                        .HasConversion(
                            id => id.Value,
                            value => QuizAnswerId.Of(value));
                    qa.HasKey("QuizId", "QuizQuestionId", nameof(QuizAnswer.Id));
                    qa.WithOwner()
                        .HasForeignKey("QuizId", "QuizQuestionId");
                    
                    
                    
                    qa.Property(x => x.Ordinal)
                        .IsRequired();
                    qa.Property(x => x.Text)
                        .HasMaxLength(1000)
                        .IsRequired()
                        .IsUnicode(true);
                    qa.Property(x => x.IsCorrect)
                        .IsRequired();


                });
                qq.Navigation(x => x.Answers).Metadata.SetField("_answers");
                qq.Navigation(x => x.Answers).UsePropertyAccessMode(PropertyAccessMode.Field);
            }
        );
    }
    private void ConfigureQuizTable(EntityTypeBuilder<Quiz> builder)
    {
        builder.ToTable("Quizzes");
        builder.Property(x => x.Id)
            .ValueGeneratedNever()
            .HasColumnName("QuizId")
            .HasConversion(
                id => id.Value,
                value => QuizId.Of(value));
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Title)
            .HasMaxLength(500)
            .IsRequired(false)
            .IsUnicode(true);
        //TODO: Source custom cofiguration
        var guidListComparer = new ValueComparer<List<Guid>>(
            (a, b) => 
                a == b || (a != null && b != null && a.SequenceEqual(b)),

            a => a == null
                ? 0
                : a.Aggregate(0, (h, g) => HashCode.Combine(h, g)),

            a => a == null ? new List<Guid>() : a.ToList()
        );
        builder.Property(e => e.ExternalId)
            .IsRequired()
            .HasColumnName("ExternalId")
            .HasColumnType("uniqueidentifier");
        builder.Property(x=>x.SourceId)
            .HasColumnName("SourceId")
            .HasConversion(
                v => JsonSerializer.Serialize(v ?? new List<Guid>(), (JsonSerializerOptions)null),
                v => JsonSerializer.Deserialize<List<Guid>>(v, (JsonSerializerOptions)null) ?? new List<Guid>()
                )
            .HasColumnType("nvarchar(max)");;
        builder.Property(x=>x.SourceId).Metadata.SetValueComparer(guidListComparer);
        
        
        builder.Property(x => x.QuizStatus)
            .HasConversion<string>()
            .HasMaxLength(50)
            .IsRequired()
            .HasDefaultValue(QuizStatus.Generating);
        
        builder.Navigation(q=>q.Tags).Metadata.SetField("_tags");
        builder.Navigation(q=>q.Tags).UsePropertyAccessMode(PropertyAccessMode.Field);
        
        builder.Navigation(q => q.Questions).Metadata.SetField("_questions");
        builder.Navigation(q=>q.Questions).UsePropertyAccessMode(PropertyAccessMode.Field);
        
    }
}