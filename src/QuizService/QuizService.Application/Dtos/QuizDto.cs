

namespace QuizService.Application.Dtos;

public class QuizDto
{
    public List<QuizQuestionDto> QuestionDtos{ get; set; }= [];
}

public  class QuizQuestionDto
{
    public string Text { get; set; }
    public string? Explanation { get; set; }
    public Guid? SourceChunkId { get; set;}
    public List<QuizAnswerDto> Answers { get; set; }= [];
}


public class QuizAnswerDto
{
    public int Ordinal {get;set;}
    public string Text {get;set;}
    public bool IsCorrect {get;set;}
}