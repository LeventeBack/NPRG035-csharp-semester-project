using Microsoft.AspNetCore.Components;

namespace QuizEdu.Src.Services;

/// <summary>
/// This service class is used to manage navigation between pages.
/// </summary>
class NavigatorService
{
    private NavigationManager? _navigationManager;

    /// <summary>
    /// Constructor to initialize the NavigatorService with the navigation manager.
    /// </summary>
    /// <param name="navigationManager"></param>
    public NavigatorService(NavigationManager navigationManager)
    {
        _navigationManager = navigationManager;
    }

    /// <summary>
    /// This method is used to navigate to a specific URL.
    /// If the navigation manager is null, this method does nothing.
    /// </summary>
    /// <param name="url"></param>
    public void NavigateTo(string url)
    {
        if (_navigationManager != null)
        {
            _navigationManager.NavigateTo(url);
        }
    }

    /// <summary>
    /// This method is used to navigate to the home page.
    /// </summary>
    public void GoToHome() => NavigateTo("/");

    /// <summary>
    /// This method is used to navigate to the select quiz page where a new game can be started.
    /// </summary>
    public void GoToSelectQuiz() => NavigateTo("/select-quiz");

    /// <summary>
    /// This method is used to navigate to the play quiz page for the given quiz id.
    /// </summary>
    /// <param name="quizId"></param>
    public void GoToQuiz(int quizId) => NavigateTo($"/play-quiz/{quizId}");

    /// <summary>
    /// This method is used to navigate to the manage quizzes page.
    /// </summary>
    public void GoToManageQuizzes() => NavigateTo("/manage-quizzes");

    /// <summary>
    /// This method is used to navigate to the manage questions page for the given quiz id.
    /// </summary>
    /// <param name="quizId"></param>
    public void GoToManageQuestions(int quizId) => NavigateTo($"/manage-questions/{quizId}");
}