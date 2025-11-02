

using QuizService.Application.Dtos;

namespace QuizService.Application.Mappers;

public static class QuizMapping
{
    public static List<ShortQuizDto> ToShortQuizDto(List<Domain.Models.Quiz.Quiz> quizzes)
    {
        List<ShortQuizDto> quizzesDto = [];
        foreach (var quiz in quizzes)
        {
            var shortQuizDto = new ShortQuizDto
            {
                QuizStatus = quiz.QuizStatus.ToString(),
                Description = quiz.ShortDescription,
                Tag = quiz.Tags.Select(x => x.Name).ToList(),
                Quantity = quiz.Questions.Count,
                DateofCreate = quiz.CreateTime
            };
            quizzesDto.Add(shortQuizDto);
        }
        return quizzesDto;
    }

    public static QuizDto ToQuizDto(Domain.Models.Quiz.Quiz quiz)
    {
        var quizDto = new QuizDto()
        {
            QuestionDtos = quiz.Questions.Select(
                q=>new QuizQuestionDto
                {
                    Text = q.Text,
                    Explanation = q.Explanation,
                    SourceChunkId = q.SourceChunkId,
                    Answers = q.Answers.Select(a=>new QuizAnswerDto
                    {
                        Ordinal = a.Ordinal,
                        Text = a.Text,
                        IsCorrect = a.IsCorrect
                    }).ToList()
                }).ToList()
        };
        return quizDto;
    }
}