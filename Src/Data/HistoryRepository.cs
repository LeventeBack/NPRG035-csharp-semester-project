using Microsoft.EntityFrameworkCore;
using QuizEdu.Src.Models;

namespace QuizEdu.Src.Data;

public class GameResultRepository
{
    private readonly ApplicationDBContext _context;

    private readonly QuizRepository _quizRepository;

    public GameResultRepository(ApplicationDBContext context)
    {
        this._context = context;
        this._quizRepository = new QuizRepository(context);
    }

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

    public async Task Create(GameResult history)
    {
        _context.Add(history);
        await _context.SaveChangesAsync();
    }

}