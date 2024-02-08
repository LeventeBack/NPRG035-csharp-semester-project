using Microsoft.EntityFrameworkCore;
using QuizEdu.Src.Models;

namespace QuizEdu.Src.Data;

/// <summary>
/// This repository class is used to perform CRUD operations on the Option table.
/// It provides methods to get all the options for a question and delete options for a question.
/// </summary>
public class OptionRepository
{
    private readonly ApplicationDBContext _context;

    /// <summary>
    /// Constructor to initialize the OptionRepository with the database context.
    /// </summary>
    /// <param name="context"></param>
    public OptionRepository(ApplicationDBContext context)
    {
        this._context = context;
    }

    /// <summary>
    /// This method is used to get all the options for a question from the database.
    /// </summary>
    /// <param name="questionId"></param>
    /// <returns></returns>
    public async Task<List<Option>> GetByQuestion(int questionId)
    {
        return await _context.Options.Where(a => a.QuestionId == questionId).ToListAsync();
    }

    /// <summary>
    /// This method is used to delete all the options for a question from the database.
    /// </summary>
    /// <param name="questionId"></param>
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