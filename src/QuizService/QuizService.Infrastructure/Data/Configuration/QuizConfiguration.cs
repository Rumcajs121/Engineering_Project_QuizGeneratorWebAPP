using Microsoft.EntityFrameworkCore.Metadata.Builders;
using QuizService.Domain.Enums;

namespace QuizService.Infrastructure.Data.Configuration;

public class QuizConfiguration:IEntityTypeConfiguration<Quiz>
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
                qq.WithOwner()
                    .HasForeignKey("QuizId");
                qq.HasKey("Id", "QuizQuestionId");
                qq.Property(x => x.Id)
                    .ValueGeneratedNever()
                    .HasConversion(
                        id => id.Value,
                        value =>QuizQuestionId.Of(value));
               qq.Property(x => x.Text)
                    .HasMaxLength(1000)                   
                    .IsRequired()                         
                    .IsUnicode(true)                     
                    .HasColumnName("Text");
                qq.Property(x => x.Explanation)
                    .HasMaxLength(2000)                   
                    .IsRequired(false)                    
                    .IsUnicode(true)                      
                    .HasColumnName("Explanation");
                qq.Property(x => x.SourceChunkId)
                    .IsRequired(false)                    
                    .HasColumnName("SourceChunkId");
                qq.OwnsMany(x => x.Answers, qa =>
                {
                    qa.ToTable("QuizAnswers");
                    qa.WithOwner().HasForeignKey("QuizQuestionId", "QuizId");
                    qa.Property(x => x.Id)
                        .HasColumnName("QuizAnswearId")
                        .ValueGeneratedNever()
                        .HasConversion(
                            id => id.Value,
                            value => QuizAnswerId.Of(value));
                    qa.Property(x => x.Ordinal);
                    //TODO:
                    qa.Property(x => x.Text);
                    //TODO:
                    qa.Property(x => x.IsCorrect);
                    //TODO:
                });
                qq.OwnsMany(x => x.Tags, t =>
                {
                    t.ToTable("QuizTags");
                    
                });
            }
        );
        
    }

    private void ConfigureQuizTable(EntityTypeBuilder<Quiz> builder)
    {
        builder.ToTable("Quizzes");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id)
            .ValueGeneratedNever()
            .HasConversion(
            id=>id.Value,
            value=>QuizId.Of(value));
        builder.Property(x => x.ShortDescription)
            .HasMaxLength(500)            
            .IsRequired(false)                   
            .IsUnicode(true);
        builder.Property(x => x.SourceId)
            .IsRequired()             
            .HasColumnType("uniqueidentifier");
        builder.Property(x=>x.QuizStatus)
            .HasConversion<string>()        
            .HasMaxLength(50)
            .IsRequired()
            .HasDefaultValue(QuizStatus.Generating);
    }
}