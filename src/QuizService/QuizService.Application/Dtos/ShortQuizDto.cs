namespace QuizService.Application.Dtos;

public class ShortQuizDto
{
    public string QuizStatus { get; set; }
    public string? Description { get; set; }
    public List<string?> Tag { get; set; }
    public int Quantity { get; set; }
    public DateTime? DateofCreate { get; set; }
}