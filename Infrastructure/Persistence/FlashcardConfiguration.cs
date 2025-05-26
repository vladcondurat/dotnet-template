using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence;

public class FlashcardConfiguration : IEntityTypeConfiguration<Flashcard>
{
    public void Configure(EntityTypeBuilder<Flashcard> builder)
    {
        builder.HasKey(f => f.FlashcardId);

        builder.Property(f => f.FlashcardId)
            .HasColumnName("flashcard_id");

        builder.Property(f => f.Question)
            .HasColumnName("question")
            .IsRequired();

        builder.Property(f => f.Answer)
            .HasColumnName("answer")
            .IsRequired();

        builder.Property(f => f.IsKnown)
            .HasColumnName("is_known")
            .IsRequired();

        builder.Property(f => f.ProjectId)
            .HasColumnName("project_id");

        builder.HasOne(f => f.Project)
            .WithMany(p => p.Flashcards)
            .HasForeignKey(f => f.ProjectId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}