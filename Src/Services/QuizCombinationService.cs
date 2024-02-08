using QuizEdu.Src.Data;
using QuizEdu.Src.Models;

namespace QuizEdu.Src.Services;

/// <summary>
/// This service is used to manage quiz combination operations.
/// </summary>
public class QuizCombinationService
{
    private QuizCombinationRepository _repository;
    private QuizRepository _quizRepository;
    private int _parentId;
    private List<Quiz> _quizzes;
    private List<QuizCombination> _quizCombinations;

    /// <summary>
    /// The list of quizzes which are showed on the UI as a list.
    /// </summary>
    public List<Quiz> QuizList { get; private set; }

    /// <summary>
    /// Constructor to initialize the QuizCombinationService with the quiz combination repository, quiz repository, and parent id.
    /// </summary>
    /// <param name="repository"></param>
    /// <param name="quizRepository"></param>
    /// <param name="parentId"></param>
    public QuizCombinationService(
        QuizCombinationRepository repository,
        QuizRepository quizRepository,
        int parentId)
    {
        _repository = repository;
        _quizRepository = quizRepository;
        _parentId = parentId;
    }

    /// <summary>
    /// This method is used to load the quiz combinations from the database.
    /// After loading the quiz combinations, it calls the Update method to update the quiz list.
    /// </summary>
    /// <returns></returns>
    public async Task LoadQuizCombinations()
    {
        _quizCombinations = await _repository.GetByParentId(_parentId);
        _quizzes = await _quizRepository.Get();
        Update();
    }

    /// <summary>
    /// This method is used to add a new combination for the given quiz
    /// </summary>
    /// <param name="childId"></param>
    /// <returns></returns>
    public async Task AddQuiz(int childId)
    {
        var quizCombination = new QuizCombination
        {
            ParentId = _parentId,
            ChildId = childId
        };
        await _repository.Add(quizCombination);
        _quizCombinations.Add(quizCombination);
        Update();
    }

    /// <summary>
    /// This method is used to remove a combination for the given quiz
    /// </summary>
    /// <param name="childId"></param>
    /// <returns></returns>
    public async Task RemoveQuiz(int childId)
    {
        var quizCombination = _quizCombinations.FirstOrDefault(a => a.ChildId == childId);
        if (quizCombination != null)
        {
            await _repository.Remove(quizCombination);
            _quizCombinations.Remove(quizCombination);
        }
        Update();
    }

    /// <summary>
    /// This method is used to update the quiz list.
    /// </summary>
    public void Update()
    {
        QuizList = GetIncludedQuizzes().Union(GetExcludedQuizzes()).ToList();
    }

    /// <summary>
    /// This method is used to get the quizzes which are included in the quiz combinations.
    /// </summary>
    /// <returns>The list of included quizzes</returns>
    private List<Quiz> GetIncludedQuizzes()
    {
        return GetQuizListByFiler(true);
    }

    /// <summary>
    /// This method is used to get the quizzes which are excluded in the quiz combinations.
    /// </summary>
    /// <returns>The list of excluded quizzes</returns>
    private List<Quiz> GetExcludedQuizzes()
    {
        return GetQuizListByFiler(false);
    }

    /// <summary>
    /// This method is a helper method to get the quizzes by filter (included/excluded).
    /// </summary>
    /// <param name="include"></param>
    /// <returns>The list of quizzes</returns>
    private List<Quiz> GetQuizListByFiler(bool include)
    {
        List<Quiz> excludedQuizzes = new List<Quiz>();
        foreach (var quiz in _quizzes)
        {
            if (IsQuizIncluded(quiz.Id) == include)
            {
                excludedQuizzes.Add(quiz);
            }
        }
        return excludedQuizzes;
    }

    /// <summary>
    /// This method is used to check if the quiz is included in the quiz combinations.
    /// </summary>
    /// <param name="quizId"></param>
    /// <returns>True if the quiz is included in the current combinations, otherwise false</returns>
    public bool IsQuizIncluded(int quizId)
    {
        return _quizCombinations.Any(a => a.ChildId == quizId);
    }

}