namespace Application.DTOs;

public class GenerateQuizRequest
{
    public Guid ProjectId { get; set; }
    public string Topic { get; set; } = string.Empty;
    public string Difficulty { get; set; } = string.Empty;
    public int NumberOfQuestions { get; set; }
}