namespace Domain.Entities;

public class Flashcard
{
    public Guid FlashcardId { get; set; }
    public string Question { get; set; } = null!;
    public string Answer { get; set; } = null!;
    public bool IsKnown { get; set; }

    public Guid ProjectId { get; set; }
    public Project Project { get; set; } = null!;
}