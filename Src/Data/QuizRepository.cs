using Microsoft.EntityFrameworkCore;
using QuizEdu.Src.Models;

namespace QuizEdu.Src.Data;

public class QuizRepository
{
    private readonly ApplicationDBContext _context;
    private readonly QuestionRepository _questionRepository;
    private readonly QuizCombinationRepository _quizCombinationRepository;

    public QuizRepository(ApplicationDBContext context)
    {
        this._context = context;
        this._questionRepository = new QuestionRepository(context);
        this._quizCombinationRepository = new QuizCombinationRepository(context);
    }

    public async Task<List<Quiz>> Get()
    {
        List<Quiz> quizzes = await _context.Quizzes.ToListAsync();
        foreach (var quiz in quizzes)
        {
            quiz.Questions = await _questionRepository.GetByQuiz(quiz);
        }
        return quizzes;
    }

    public async Task<Quiz> Get(int id)
    {
        Quiz quiz = await _context.Quizzes.FirstOrDefaultAsync(a => a.Id == id);

        quiz.Questions = await _questionRepository.GetByQuiz(quiz);

        return quiz;
    }

    public async Task<int> Create(Quiz Quiz)
    {
        _context.Add(Quiz);
        await _context.SaveChangesAsync();
        return Quiz.Id;
    }

    public async Task Update(Quiz Quiz)
    {
        _context.Quizzes.Where(a => a.Id == Quiz.Id).FirstOrDefault().Title = Quiz.Title;
        await _context.SaveChangesAsync();
    }

    public async Task Delete(int id)
    {
        var Quiz = await _context.Quizzes.FindAsync(id);

        await _questionRepository.DeleteByQuiz(id);
        _context.Quizzes.Remove(Quiz);
        await _quizCombinationRepository.ClearPairsByChildId(id);

        await _context.SaveChangesAsync();
    }

    public async Task<bool> IsQuizPlayable(int id)
    {
        var quiz = await _context.Quizzes.FirstOrDefaultAsync(a => a.Id == id);
        if (quiz == null)
        {
            return false;
        }
        if (quiz.IsNormalQuiz())
        {
            return quiz.Questions.Count >= quiz.RoundCount && quiz.Questions.Count > 0;
        }

        int count = 0;
        var quizCombinations = await _quizCombinationRepository.GetByParentId(id);
        foreach (var combination in quizCombinations)
        {
            var childQuiz = await _questionRepository.GetQuestionCountByQuizId(combination.ChildId);
            count += childQuiz;
        }

        return count >= quiz.RoundCount && count > 0;
    }

    public async Task<List<Question>> GetCombinedQuestions(Quiz quiz)
    {
        return await _questionRepository.GetCombinedQuestions(quiz);
    }
}