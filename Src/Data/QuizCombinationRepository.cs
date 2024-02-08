using Microsoft.EntityFrameworkCore;
using QuizEdu.Src.Models;

namespace QuizEdu.Src.Data;

/// <summary>
/// This repository class is used to perform CRUD operations on the QuizCombination table.
/// It provides methods to add a new combination, remove a combination, get all combinations by parent id, and clear pairs by child id.
/// </summary>
public class QuizCombinationRepository
{
    private readonly ApplicationDBContext _context;

    /// <summary>
    /// Constructor to initialize the QuizCombinationRepository with the database context.
    /// </summary>
    /// <param name="context"></param>
    public QuizCombinationRepository(ApplicationDBContext context)
    {
        this._context = context;
    }

    /// <summary>
    /// This method is used to add a new combination to the database.
    /// </summary>
    /// <param name="quizCombination"></param>
    /// <returns></returns>
    public async Task Add(QuizCombination quizCombination)
    {
        _context.Add(quizCombination);
        await _context.SaveChangesAsync();
    }

    /// <summary>
    /// This method is used to remove a combination from the database.
    /// </summary>
    public async Task Remove(QuizCombination quizCombination)
    {
        _context.QuizCombinations.Remove(quizCombination);
        await _context.SaveChangesAsync();
    }

    /// <summary>
    /// This method is used to get all the combinations by parent id from the database.
    /// </summary>
    /// <param name="parentId"></param>
    /// <returns>The list of combinations</returns>
    public async Task<List<QuizCombination>> GetByParentId(int parentId)
    {
        return await _context.QuizCombinations.Where(a => a.ParentId == parentId).ToListAsync();
    }

    /// <summary>
    /// This method is used to clear pairs by child id from the database.
    /// </summary>
    /// <param name="childId"></param>
    /// <returns></returns>
    public async Task ClearPairsByChildId(int childId)
    {
        _context.QuizCombinations.RemoveRange(_context.QuizCombinations.Where(a => a.ChildId == childId));
        await _context.SaveChangesAsync();
    }
}