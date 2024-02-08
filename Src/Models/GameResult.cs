using System.ComponentModel.DataAnnotations.Schema;

namespace QuizEdu.Src.Models;

/// <summary>
/// This class represents a game result in the database.
/// It contains the user name, quiz id, score, and date of the game result.
/// It also contains the name of the quiz, which is not added to the database.
/// </summary>
public class GameResult
{
    public const string UNKNOWN_QUIZ_NAME = "(Unknown)";
    public const string DEFAULT_USER_NAME = "Guest User";
    public int Id { get; set; }
    public string UserName { get; set; }
    public int QuizId { get; set; }
    public int Score { get; set; }
    public DateTime Date { get; set; }

    // This property is not added to the database
    [NotMapped]
    public string QuizName { get; set; }

    /// <summary>
    /// Constructor to initialize the game result with default values.
    /// </summary>
    public GameResult()
    {
        UserName = DEFAULT_USER_NAME;
        QuizId = 0;
        Score = 0;
        Date = DateTime.Now;
        QuizName = UNKNOWN_QUIZ_NAME;
    }
}
