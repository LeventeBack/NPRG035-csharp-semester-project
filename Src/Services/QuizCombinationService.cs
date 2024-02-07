using QuizEdu.Src.Data;
using QuizEdu.Src.Models;

namespace QuizEdu.Src.Services;


public class QuizCombinationService
{
    private QuizCombinationRepository _repository;
    private QuizRepository _quizRepository;
    private int _parentId;
    private List<Quiz> _quizzes;
    private List<QuizCombination> _quizCombinations;
    public List<Quiz> QuizList { get; private set; }

    public QuizCombinationService(
        QuizCombinationRepository repository,
        QuizRepository quizRepository,
        int parentId)
    {
        _repository = repository;
        _quizRepository = quizRepository;
        _parentId = parentId;
    }

    public async Task LoadQuizCombinations()
    {
        _quizCombinations = await _repository.GetByParentId(_parentId);
        _quizzes = await _quizRepository.Get();
        Update();
    }

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

    public void Update()
    {
        QuizList = GetIncludedQuizzes().Union(GetExcludedQuizzes()).ToList();
    }

    private List<Quiz> GetIncludedQuizzes()
    {
        return GetQuizListByFiler(true);
    }

    private List<Quiz> GetExcludedQuizzes()
    {
        return GetQuizListByFiler(false);
    }

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

    public bool IsQuizIncluded(int quizId)
    {
        return _quizCombinations.Any(a => a.ChildId == quizId);
    }

}