namespace QuizEdu.Src.Models;

public class Quiz
{
    public int Id { get; set; }
    public string Title { get; set; }
    public List<Question> Questions { get; set; }

    public Quiz()
    {
        Title = "";
        Questions = new List<Question>();
    }
}
