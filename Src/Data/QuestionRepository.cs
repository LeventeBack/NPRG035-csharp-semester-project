using Microsoft.EntityFrameworkCore;
using QuizEdu.Src.Models;

namespace QuizEdu.Src.Data;

public class QuestionRepository
{
    private readonly ApplicationDBContext _context;
    private readonly OptionRepository _optionRepository;

    private readonly QuizCombinationRepository _quizCombinationRepository;

    public QuestionRepository(ApplicationDBContext context)
    {
        this._context = context;
        this._optionRepository = new OptionRepository(context);
        this._quizCombinationRepository = new QuizCombinationRepository(context);
    }
    public async Task<List<Question>> GetByQuiz(Quiz quiz)
    {
        List<Question> questions = await GetQuestionByQuizId(quiz.Id);

        foreach (var question in questions)
        {
            question.Options = await _optionRepository.GetByQuestion(question.Id);
        }

        return questions;
    }

    public async Task<int> GetQuestionCountByQuizId(int quizId)
    {
        return await _context.Questions.Where(a => a.QuizId == quizId).CountAsync();
    }

    public async Task<List<Question>> GetCombinedQuestions(Quiz quiz)
    {
        if (quiz.IsNormalQuiz())
        {
            return await GetQuestionByQuizId(quiz.Id);
        }

        List<QuizCombination> quizCombinations = await _quizCombinationRepository.GetByParentId(quiz.Id);

        List<Question> questions = new List<Question>();
        foreach (var combination in quizCombinations)
        {
            List<Question> childQuestions = await GetQuestionByQuizId(combination.ChildId);
            questions.AddRange(childQuestions);
        }

        return questions;
    }

    private async Task<List<Question>> GetQuestionByQuizId(int quizId)
    {
        return await _context.Questions.Where(a => a.QuizId == quizId).ToListAsync();
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

    public async Task Delete(Question question)
    {
        _context.Questions.Remove(question);
        await _context.SaveChangesAsync();
        await _optionRepository.DeleteByQuestion(question.Id);
    }

    public async Task DeleteByQuiz(int quizId)
    {
        List<Question> questions = await _context.Questions.Where(a => a.QuizId == quizId).ToListAsync();

        foreach (var question in questions)
        {
            await Delete(question);
        }

        await _context.SaveChangesAsync();
    }
}