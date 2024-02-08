namespace QuizEdu.Src.Models;

/// <summary>
/// This class represents a quiz in the database.
/// It contains the id, title, type, round count, and a list of questions.
/// It also contains methods to check if the quiz is a normal quiz or a combined quiz.
/// </summary>
public class Quiz
{
    public enum QuizType
    {
        Normal = 0,
        Combined = 1
    }

    // Default values
    public const QuizType DEFAULT_QUIZ_TYPE = QuizType.Normal;
    public const int DEFAULT_ROUND_COUNT = 10;

    // Properties
    public int Id { get; set; }
    public string Title { get; set; }
    public int Type { get; set; }
    public int RoundCount { get; set; }
    public List<Question> Questions { get; set; }

    /// <summary>
    /// Constructor to initialize the Quiz with default values.
    /// </summary>
    public Quiz()
    {
        Title = "";
        Questions = new List<Question>();
        Type = (int)DEFAULT_QUIZ_TYPE;
        RoundCount = DEFAULT_ROUND_COUNT;
    }

    /// <summary>
    /// This method is used to check if the quiz is a normal quiz.
    /// </summary>
    /// <returns>true if the quiz is a normal quiz, false otherwise</returns>
    public bool IsNormalQuiz()
    {
        return Type == (int)QuizType.Normal;
    }

    /// <summary>
    /// This method is used to check if the quiz is a combined quiz.
    /// </summary>
    /// <returns></returns>
    public bool IsCombinedQuiz()
    {
        return Type == (int)QuizType.Combined;
    }
}
