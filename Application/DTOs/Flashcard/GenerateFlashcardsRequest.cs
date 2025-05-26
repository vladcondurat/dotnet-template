namespace Application.DTOs;

public class GenerateFlashcardsRequest
{
    public Guid ProjectId { get; set; }
    public string Difficulty { get; set; } = string.Empty;
    public int NumberOfFlashcards { get; set; }
    public string? FocusArea { get; set; }
}