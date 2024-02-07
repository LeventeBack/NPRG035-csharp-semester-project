using Microsoft.EntityFrameworkCore;
using QuizEdu.Src.Models;

namespace QuizEdu.Src.Data;

public class QuizCombinationRepository
{
    private readonly ApplicationDBContext _context;

    private readonly QuizCombinationRepository _repository;

    public QuizCombinationRepository(ApplicationDBContext context)
    {
        this._context = context;
        this._repository = new QuizCombinationRepository(context);
    }

    public async Task Add(QuizCombination quizCombination)
    {
        _context.Add(quizCombination);
        await _context.SaveChangesAsync();
    }

    public async Task Remove(QuizCombination quizCombination)
    {
        _context.QuizCombinations.Remove(quizCombination);
        await _context.SaveChangesAsync();
    }

    public async Task<List<QuizCombination>> GetByParentId(int parentId)
    {
        return await _context.QuizCombinations.Where(a => a.ParentId == parentId).ToListAsync();
    }
}