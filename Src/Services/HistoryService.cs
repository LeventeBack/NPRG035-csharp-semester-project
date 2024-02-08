using QuizEdu.Src.Models;
using QuizEdu.Src.Data;
using Microsoft.AspNetCore.Components;

namespace QuizEdu.Src.Services;

/// <summary>
/// This service class is used to manage history-related operations.
/// </summary>
public class HistoryService
{
    private GameResultRepository _repository;
    private NavigatorService? _navigator;

    /// <summary>
    /// The list of game results.
    /// </summary>
    public List<GameResult>? GameResults;

    /// <summary>
    /// The username of the user who played the game. 
    /// This name is used to save the game result.
    /// </summary>
    public string UserName { get; set; } = GameResult.DEFAULT_USER_NAME;

    /// <summary>
    /// Constructor to initialize the HistoryService with the game result repository and navigation manager.
    /// </summary>
    /// <param name="repository"></param>
    /// <param name="navigationManager"></param>
    public HistoryService(
        GameResultRepository repository,
        NavigationManager navigationManager = null
    )
    {
        _repository = repository;
        _navigator = new NavigatorService(navigationManager);
    }

    /// <summary>
    /// This method is used to load the history of game results from the database.
    /// </summary>
    /// <returns></returns>
    public async Task LoadHistory()
    {
        GameResults = await _repository.GetAll();
    }

    /// <summary>
    /// This method is used to save the current game to the history.
    /// After saving the game, it navigates to the select quiz page.
    /// </summary>
    /// <param name="quizId"></param>
    /// <param name="score"></param>
    public async void InitSaveGameResult(int quizId, int score)
    {
        await SaveGameResult(quizId, score);
        _navigator.GoToSelectQuiz();
    }

    /// <summary>
    /// This method is used to save the game result to the database.
    /// </summary>
    /// <param name="quizId"></param>
    /// <param name="score"></param>
    /// <returns></returns>
    private async Task SaveGameResult(int quizId, int score)
    {
        GameResult gameResult = new GameResult
        {
            UserName = UserName,
            QuizId = quizId,
            Score = score,
        };

        await _repository.Create(gameResult);
    }
}