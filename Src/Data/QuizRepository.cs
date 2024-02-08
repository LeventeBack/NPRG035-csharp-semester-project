using Microsoft.EntityFrameworkCore;
using QuizEdu.Src.Models;

namespace QuizEdu.Src.Data;

/// <summary>
/// This repository class is used to perform CRUD operations on the Quiz table.
/// It provides methods to get all the quizzes, get a quiz by id, create a new quiz, update a quiz, and delete a quiz.
/// </summary>
public class QuizRepository
{
    private readonly ApplicationDBContext _context;
    private readonly QuestionRepository _questionRepository;
    private readonly QuizCombinationRepository _quizCombinationRepository;

    /// <summary>
    /// Constructor to initialize the QuizRepository with the database context.
    /// </summary>
    public QuizRepository(ApplicationDBContext context)
    {
        this._context = context;
        this._questionRepository = new QuestionRepository(context);
        this._quizCombinationRepository = new QuizCombinationRepository(context);
    }

    /// <summary>
    /// This method is used to get all the quizzes from the database.
    /// It also fetches the questions for each quiz.
    /// </summary>
    /// <returns>The list of quizzes</returns>
    public async Task<List<Quiz>> Get()
    {
        List<Quiz> quizzes = await _context.Quizzes.ToListAsync();
        foreach (var quiz in quizzes)
        {
            quiz.Questions = await _questionRepository.GetByQuiz(quiz);
        }
        return quizzes;
    }

    /// <summary>
    /// This method is used to get a quiz by id from the database.
    /// It also fetches the questions for the quiz.
    /// </summary>
    /// <param name="id"></param>
    /// <returns>The quiz with the given id</returns>
    public async Task<Quiz> Get(int id)
    {
        Quiz quiz = await _context.Quizzes.FirstOrDefaultAsync(a => a.Id == id);

        quiz.Questions = await _questionRepository.GetByQuiz(quiz);

        return quiz;
    }

    /// <summary>
    /// This method is used to create a new quiz in the database.
    /// </summary>
    /// <param name="Quiz"></param>
    /// <returns></returns>
    public async Task<int> Create(Quiz Quiz)
    {
        _context.Add(Quiz);
        await _context.SaveChangesAsync();
        return Quiz.Id;
    }

    /// <summary>
    /// This method is used to update a quiz in the database.
    /// </summary>
    /// <param name="Quiz"></param>
    /// <returns></returns>
    public async Task Update(Quiz Quiz)
    {
        _context.Quizzes.Where(a => a.Id == Quiz.Id).FirstOrDefault().Title = Quiz.Title;
        await _context.SaveChangesAsync();
    }

    /// <summary>
    /// This method is used to delete a quiz from the database.
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public async Task Delete(int id)
    {
        var Quiz = await _context.Quizzes.FindAsync(id);

        await _questionRepository.DeleteByQuiz(id);
        _context.Quizzes.Remove(Quiz);
        await _quizCombinationRepository.ClearPairsByChildId(id);

        await _context.SaveChangesAsync();
    }

    /// <summary>
    /// This method is used to check if a quiz is playable based on the number of questions and rounds.
    /// For a normal quiz, the number of questions should be greater than or equal to the number of rounds.
    /// For a combined quiz, the number of questions from all the child quizzes should be greater than or equal to the number of rounds.
    /// </summary>
    /// <param name="id"></param>
    /// <returns>true if the quiz is playable, false otherwise</returns>
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

    /// <summary>
    /// This method is used to get the combined questions for a quiz.
    /// </summary>
    /// <param name="quiz"></param>
    /// <returns>The list of combined questions for the quiz</returns>
    public async Task<List<Question>> GetCombinedQuestions(Quiz quiz)
    {
        return await _questionRepository.GetCombinedQuestions(quiz);
    }
}