namespace Domain.Entities;

public class Project
{
    public Guid Id { get; set; }
    public string Title { get; set; } = null!;
    public string Description { get; set; } = null!;
    public string Summary { get; set; } = null!;
    public DateTime CreatedAt { get; set; }
    
    
    public Guid UserId { get; set; }
    public User User { get; set; } = null!;

    public IEnumerable<Quiz> Quizzes { get; set; } = new List<Quiz>();
    public IEnumerable<Flashcard> Flashcards { get; set; } = new List<Flashcard>();
}