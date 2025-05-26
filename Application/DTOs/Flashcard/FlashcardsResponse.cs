namespace Application.DTOs;

public class FlashcardsResponse
{
    public IEnumerable<FlashcardDto> Flashcards { get; set; } = null!;
    public int NumberOfFlashcards { get; set; }
} 