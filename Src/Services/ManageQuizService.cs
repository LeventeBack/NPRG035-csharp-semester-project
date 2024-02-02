using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using QuizEdu.Src.Models;

namespace QuizEdu.Src.Services;

public class ManageQuizService
{
    private QuizRepository? _repository;
    private NavigatorService _navigator;
    private JsInteractionService _jsInteraction;

    public List<Quiz>? Quizzes;

    public ManageQuizService(
        QuizRepository repository,
        NavigationManager navigationManager,
        IJSRuntime jsRuntime = null
    )
    {
        _repository = repository;
        _navigator = new NavigatorService(navigationManager);
        _jsInteraction = new JsInteractionService(jsRuntime);
    }

    public async Task LoadQuizzes()
    {
        Quizzes = await _repository.Get();
    }

    public void InitStartQuiz(Quiz quiz)
    {
        if (quiz != null)
        {
            _navigator.GoToQuiz(quiz.Id);
        }
    }

    public async Task InitCreateQuiz()
    {
        Quiz quiz = new Quiz() { Title = "New Quiz" };
        int quizId = await _repository.Create(quiz);
        _navigator.GoToManageQuestions(quizId);
    }
    public void InitEditQuiz(Quiz quiz)
    {
        if (quiz != null)
        {
            _navigator.GoToManageQuestions(quiz.Id);
        }
    }

    public async Task InitDeleteQuiz(Quiz quiz)
    {
        var confirm = await _jsInteraction.ConfirmDelete(quiz.Title);
        if (confirm)
        {
            await _repository.Delete(quiz.Id);
            Quizzes.Remove(quiz);
        }
    }

    public async Task LoadNonEmptyQuizzes()
    {
        await LoadQuizzes();
        Quizzes = Quizzes.Where(q => q.Questions.Count > 0).ToList();
    }
}