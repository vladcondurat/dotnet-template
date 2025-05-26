using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence;

public class QuizConfiguration : IEntityTypeConfiguration<Quiz>
{
    public void Configure(EntityTypeBuilder<Quiz> builder)
    {
        builder.HasKey(q => q.QuizId);

        builder.Property(q => q.QuizId)
            .HasColumnName("quiz_id");

        builder.Property(q => q.Title)
            .HasColumnName("title")
            .IsRequired();

        builder.Property(q => q.Description)
            .HasColumnName("description");

        builder.Property(q => q.Timer)
            .HasColumnName("timer")
            .IsRequired();

        builder.Property(q => q.NumberOfQuestions)
            .HasColumnName("number_of_questions")
            .IsRequired();

        builder.Property(q => q.Difficulty)
            .HasColumnName("difficulty");

        builder.Property(q => q.CreatedAt)
            .HasColumnName("created_at")
            .IsRequired();

        builder.Property(q => q.ProjectId)
            .HasColumnName("project_id");

        builder.HasOne(q => q.Project)
            .WithMany(p => p.Quizzes)
            .HasForeignKey(q => q.ProjectId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(q => q.Questions)
            .WithOne(qq => qq.Quiz)
            .HasForeignKey(qq => qq.QuizId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}