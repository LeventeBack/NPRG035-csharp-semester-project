using Microsoft.EntityFrameworkCore;
using QuizEdu.Src.Models;

namespace QuizEdu.Src.Services;

public class OptionRepository
{
    private readonly ApplicationDBContext _context;

    public OptionRepository(ApplicationDBContext context)
    {
        this._context = context;
    }

    public async Task<List<Option>> GetByQuestion(int questionId)
    {
        return await _context.Options.Where(a => a.QuestionId == questionId).ToListAsync();
    }

    public async Task DeleteByQuestion(int questionId)
    {
        List<Option> options = await _context.Options.Where(a => a.QuestionId == questionId).ToListAsync();

        foreach (var option in options)
        {
            _context.Options.Remove(option);
        }

        await _context.SaveChangesAsync();
    }

}