namespace QuizEdu.Src.Models;

/// <summary>
/// This class represents an option in the database.
/// It contains the id, text, whether it is correct, and the question id.
/// </summary>
public class Option
{
    public int Id { get; set; }
    public string Text { get; set; }
    public bool IsCorrect { get; set; }
    public int QuestionId { get; set; }

    /// <summary>
    /// Constructor to initialize the option with default values.
    /// </summary>
    public Option()
    {
        Text = "";
        IsCorrect = false;
    }
}
