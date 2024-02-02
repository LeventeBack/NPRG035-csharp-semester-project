namespace QuizEdu.Src.Models;

public class Quiz
{
    public int Id { get; set; }
    public string Title { get; set; }

    // Type = 0 - normal quiz with questions 
    // Type = 1 - combined quiz with other quizzes 
    public int Type { get; set; } = 0;

    public int RoundCount { get; set; } = 0;

    public List<Question> Questions { get; set; }

    public Quiz()
    {
        Title = "";
        Questions = new List<Question>();
    }
}
