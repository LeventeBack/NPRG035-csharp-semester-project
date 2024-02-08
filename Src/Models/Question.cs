using QuizEdu.Src.Services;

namespace QuizEdu.Src.Models;

/// <summary>
/// This class represents a question in the database.
/// It contains the id, text, quiz id, and a list of options.
/// </summary>
public class Question
{
    public int Id { get; set; } = 0;
    public string Text { get; set; }
    public int QuizId { get; set; }
    public List<Option> Options { get; set; }

    /// <summary>
    /// Constructor to initialize the question with default values.
    /// </summary>
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