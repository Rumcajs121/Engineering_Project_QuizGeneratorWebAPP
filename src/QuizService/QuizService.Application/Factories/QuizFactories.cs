

using QuizService.Application.Dtos;

namespace QuizService.Application.Factories;

public static class QuizFactories
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
}