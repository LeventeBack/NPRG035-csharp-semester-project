using Microsoft.AspNetCore.Components;

namespace QuizEdu.Src.Services;

class NavigatorService
{
    private NavigationManager? _navigationManager;

    public NavigatorService(NavigationManager navigationManager)
    {
        _navigationManager = navigationManager;
    }

    public void NavigateTo(string url)
    {
        if (_navigationManager != null)
        {
            _navigationManager.NavigateTo(url);
        }
    }

    public void GoToHome() => NavigateTo("/");
    public void GoToSelectQuiz() => NavigateTo("/select-quiz");
    public void GoToQuiz(int quizId) => NavigateTo($"/play-quiz/{quizId}");
    public void GoToManageQuizzes() => NavigateTo("/manage-quizzes");
    public void GoToManageQuestions(int quizId) => NavigateTo($"/manage-questions/{quizId}");
}