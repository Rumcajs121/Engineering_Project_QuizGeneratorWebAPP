using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace QuizService.Infrastructure.Data.Configuration;

public class TagConfiguration:IEntityTypeConfiguration<Tag>
{
    public void Configure(EntityTypeBuilder<Tag> builder)
    {
        builder.ToTable("Tags");
        builder.Property(t=>t.Id)
            .ValueGeneratedNever()
            .HasColumnName("TagId")
            .HasConversion(
                id=>id.Value,
                value=>QuizTagId.Of(value));
        builder.HasKey(t=>t.Id);
        builder.HasMany(q => q.Quizzes)
            .WithMany(q => q.Tags)
            .UsingEntity<Dictionary<string, object>>(
                "QuizTag",
                r => r
                    .HasOne<Quiz>()
                    .WithMany()
                    .HasForeignKey("QuizId")
                    .OnDelete(DeleteBehavior.Cascade),
                r => r
                    .HasOne<Tag>()
                    .WithMany()
                    .HasForeignKey("TagId")
                    .OnDelete(DeleteBehavior.Cascade),
                je =>
                {
                    je.HasKey("QuizId","TagId");
                    je.HasIndex("TagId");
                    je.ToTable("QuizTag");
                });
            
            builder.Property(x => x.Name)
                .HasMaxLength(50)
                .IsRequired()
                .IsUnicode(true);
            builder
                .HasIndex(x => x.Name)
                .IsUnique();
            builder.Navigation(q => q.Quizzes).Metadata.SetField("_quizzes");
            builder.Navigation(q=>q.Quizzes).UsePropertyAccessMode(PropertyAccessMode.Field);
            
    }
}