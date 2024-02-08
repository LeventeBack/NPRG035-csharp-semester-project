using Microsoft.EntityFrameworkCore;
using QuizEdu.Src.Models;

namespace QuizEdu.Src.Data;

/// <summary>
/// This repository class is used to perform CRUD operations on the GameResult table.
/// It provides methods to get all the game results and create a new game result.
/// </summary>
public class GameResultRepository
{
    private readonly ApplicationDBContext _context;
    private readonly QuizRepository _quizRepository;

    /// <summary>
    /// Constructor to initialize the GameResultRepository with the database context.
    /// </summary>
    /// <param name="context">The database context for the application</param>
    public GameResultRepository(ApplicationDBContext context)
    {
        this._context = context;
        this._quizRepository = new QuizRepository(context);
    }

    /// <summary>
    /// This method is used to get all the game results from the database.
    /// It also fetches the quiz name for each game result.
    /// </summary>
    /// <returns>A list of game results</returns>
    public async Task<List<GameResult>> GetAll()
    {
        List<GameResult> history = await _context.GameResults.ToListAsync();

        foreach (var item in history)
        {
            Quiz quiz = await _quizRepository.Get(item.QuizId);
            if (quiz == null)
            {
                item.QuizName = GameResult.UNKNOWN_QUIZ_NAME;
            }
            else
            {
                item.QuizName = quiz.Title;
            }
        }

        return history;
    }

    /// <summary>
    /// This method is used to create a new game result in the database.
    /// </summary>
    /// <param name="history"></param>
    /// <returns></returns>
    public async Task Create(GameResult history)
    {
        _context.Add(history);
        await _context.SaveChangesAsync();
    }

}