using Microsoft.EntityFrameworkCore;
using QuizEdu.Src.Models;

namespace QuizEdu.Src.Data;

/// <summary>
/// This repository class is used to perform CRUD operations on the Question table.
/// It provides methods to get all the questions for a quiz, create a new question, edit a question, update a question, and delete a question.
/// </summary>
public class QuestionRepository
{
    private readonly ApplicationDBContext _context;
    private readonly OptionRepository _optionRepository;

    private readonly QuizCombinationRepository _quizCombinationRepository;

    /// <summary>
    /// Constructor to initialize the QuestionRepository with the database context.
    /// </summary>
    /// <param name="context"></param>
    public QuestionRepository(ApplicationDBContext context)
    {
        this._context = context;
        this._optionRepository = new OptionRepository(context);
        this._quizCombinationRepository = new QuizCombinationRepository(context);
    }

    /// <summary>
    /// This method is used to get all the questions for a quiz from the database.
    /// It also fetches the options for each question.
    /// </summary>
    /// <param name="quiz"></param>
    /// <returns>The list of questions for the quiz</returns>
    public async Task<List<Question>> GetByQuiz(Quiz quiz)
    {
        List<Question> questions = await GetQuestionByQuizId(quiz.Id);

        foreach (var question in questions)
        {
            question.Options = await _optionRepository.GetByQuestion(question.Id);
        }

        return questions;
    }

    /// <summary>
    /// This method is used to get the count of questions for a quiz from the database.
    /// </summary>
    /// <param name="quizId"></param>
    /// <returns>The count of questions for the quiz</returns>
    public async Task<int> GetQuestionCountByQuizId(int quizId)
    {
        return await _context.Questions.Where(a => a.QuizId == quizId).CountAsync();
    }

    /// <summary>
    /// This method is used to get all the questions for a quiz from the database.
    /// </summary>
    /// <param name="quiz"></param>
    /// <returns>Returns the list of questions for the quiz</returns>
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

    /// <summary>
    /// This method is used to get all the questions for a quiz from the database.
    /// </summary>
    /// <param name="quizId"></param>
    /// <returns>Returns the list of questions for the quiz</returns>
    private async Task<List<Question>> GetQuestionByQuizId(int quizId)
    {
        return await _context.Questions.Where(a => a.QuizId == quizId).ToListAsync();
    }

    /// <summary>
    /// This method is used to create a new question in the database.
    /// </summary>
    /// <param name="question"></param>
    /// <returns></returns>
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

    /// <summary>
    /// This method is used to edit a question in the database.
    /// </summary>
    /// <param name="question"></param>
    /// <returns>Returns the updated question</returns>
    public async Task<Question> Edit(Question question)
    {
        _context.Entry(question).State = EntityState.Modified;
        await _context.SaveChangesAsync();
        return question;
    }

    /// <summary>
    /// This method is used to update a question and its options in the database.
    /// </summary>
    /// <param name="question"></param>
    /// <returns></returns>
    public async Task Update(Question question)
    {
        _context.Entry(question).State = EntityState.Modified;
        foreach (var option in question.Options)
        {
            _context.Entry(option).State = EntityState.Modified;
        }
        await _context.SaveChangesAsync();
    }

    /// <summary>
    /// This method is used to delete a question and its options from the database.
    /// </summary>
    /// <param name="question"></param>
    /// <returns></returns>
    public async Task Delete(Question question)
    {
        _context.Questions.Remove(question);
        await _context.SaveChangesAsync();
        await _optionRepository.DeleteByQuestion(question.Id);
    }

    /// <summary>
    /// This method is used to delete all the questions for a quiz from the database.
    /// </summary>
    /// <param name="quizId"></param>
    /// <returns></returns>
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