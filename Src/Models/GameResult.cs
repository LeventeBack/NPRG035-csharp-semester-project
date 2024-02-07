using System.ComponentModel.DataAnnotations.Schema;

namespace QuizEdu.Src.Models;

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

    public GameResult()
    {
        UserName = DEFAULT_USER_NAME;
        QuizId = 0;
        Score = 0;
        Date = DateTime.Now;
        QuizName = UNKNOWN_QUIZ_NAME;
    }
}
