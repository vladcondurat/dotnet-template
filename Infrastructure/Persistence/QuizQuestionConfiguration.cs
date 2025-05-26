using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence;

public class QuizQuestionConfiguration : IEntityTypeConfiguration<QuizQuestion>
{
    public void Configure(EntityTypeBuilder<QuizQuestion> builder)
    {
        builder.HasKey(qq => qq.Id);

        builder.Property(qq => qq.Id)
            .HasColumnName("quiz_question_id");

        builder.Property(qq => qq.Question)
            .HasColumnName("question")
            .IsRequired();
        
        builder.Property(qq => qq.CorrectAnswer)
            .HasColumnName("correct_answer")
            .IsRequired();
        
        builder.Property(qq => qq.IncorrectAnswers)
            .HasColumnName("incorrect_answers")
            .IsRequired();
        
        builder.Property(qq => qq.QuizId)
            .HasColumnName("quiz_id");

        builder.HasOne(qq => qq.Quiz)
            .WithMany(q => q.Questions)
            .HasForeignKey(qq => qq.QuizId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}