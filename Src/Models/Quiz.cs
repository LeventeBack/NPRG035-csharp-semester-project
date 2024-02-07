namespace QuizEdu.Src.Models;

public class Quiz
{
    public const int DEFAULT_QUIZ_TYPE = 0;
    public const int DEFAULT_ROUND_COUNT = 10;

    public int Id { get; set; }
    public string Title { get; set; }

    // Type = 0 - normal quiz with questions 
    // Type = 1 - combined quiz with other quizzes 
    public int Type { get; set; }

    public int RoundCount { get; set; }

    public List<Question> Questions { get; set; }

    public Quiz()
    {
        Title = "";
        Questions = new List<Question>();
        Type = DEFAULT_QUIZ_TYPE;
        RoundCount = DEFAULT_ROUND_COUNT;
    }
}
