using Microsoft.EntityFrameworkCore;
using QuizEdu.Src.Models;

namespace QuizEdu.Src.Services;

public class QuestionRepository
{
    private readonly ApplicationDBContext _context;
    private readonly OptionRepository _optionRepository;

    public QuestionRepository(ApplicationDBContext context)
    {
        this._context = context;
        this._optionRepository = new OptionRepository(context);
    }
    public async Task<List<Question>> GetByQuiz(int quizId)
    {
        List<Question> questions = await _context.Questions.Where(a => a.QuizId == quizId).ToListAsync();

        foreach (var question in questions)
        {
            question.Options = await _optionRepository.GetByQuestion(question.Id);
        }

        return questions;
    }

    public async Task Create(Question question)
    {
        _context.Add(question);
        foreach (var option in question.Options)
        {
            option.QuestionId = question.Id;
            _context.Add(option);
        }
        await _context.SaveChangesAsync();
    }

    public async Task<Question> Edit(Question question)
    {
        _context.Entry(question).State = EntityState.Modified;
        await _context.SaveChangesAsync();
        return question;
    }

    public async Task Update(Question question)
    {
        _context.Entry(question).State = EntityState.Modified;
        foreach (var option in question.Options)
        {
            _context.Entry(option).State = EntityState.Modified;
        }
        await _context.SaveChangesAsync();
    }

    public async Task DeleteByQuiz(int quizId)
    {
        List<Question> questions = await _context.Questions.Where(a => a.QuizId == quizId).ToListAsync();

        foreach (var question in questions)
        {
            await _optionRepository.DeleteByQuestion(question.Id);
            _context.Questions.Remove(question);
        }

        await _context.SaveChangesAsync();
    }
}