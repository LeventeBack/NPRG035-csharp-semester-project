using QuizEdu.Src.Services;

namespace QuizEdu.Src.Models;

public class Question
{
    public int Id { get; set; } = 0;
    public string Text { get; set; }
    public int QuizId { get; set; }
    public List<Option> Options { get; set; }

    public Question()
    {
        Text = "";
        Options = new List<Option>();
        for (int i = 0; i < QuizGamePlayService.OPTION_COUNT; i++)
        {
            Options.Add(new Option());
        }
    }
}