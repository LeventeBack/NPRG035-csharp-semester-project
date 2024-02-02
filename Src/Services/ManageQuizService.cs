using Microsoft.JSInterop;
using QuizEdu.Src.Models;
using Microsoft.AspNetCore.Components;

namespace QuizEdu.Src.Services;

public class ManageQuizService
{
    private QuizRepository _repository;
    private NavigationManager _navigationManager;
    private IJSRuntime _jsRuntime;

    public List<Quiz> Quizzes;

    public ManageQuizService(
        QuizRepository repository,
        NavigationManager navigationManager, IJSRuntime jsRuntime
    )
    {
        _repository = repository;
        _navigationManager = navigationManager;
        _jsRuntime = jsRuntime;
    }

    public async Task LoadQuizzes()
    {
        Quizzes = await _repository.Get();
    }

    public async Task InitCreateQuiz()
    {
        Quiz quiz = new Quiz() { Title = "New Quiz" };
        int quizId = await _repository.Create(quiz);
        _navigationManager.NavigateTo($"/manage-questions/{quizId}");
    }
    public void InitEditQuiz(Quiz quiz)
    {
        _navigationManager.NavigateTo($"/manage-questions/{quiz.Id}");
    }

    public async Task InitDeleteQuiz(Quiz quiz)
    {
        var confirm = await _jsRuntime.InvokeAsync<bool>("confirm", $"Are you sure you want to delete {quiz.Title}?");
        if (confirm)
        {
            await _repository.Delete(quiz.Id);
            Quizzes.Remove(quiz);
        }
    }
}