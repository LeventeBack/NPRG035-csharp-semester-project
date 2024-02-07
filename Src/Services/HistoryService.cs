using QuizEdu.Src.Models;
using QuizEdu.Src.Data;
using Microsoft.AspNetCore.Components;

namespace QuizEdu.Src.Services;

public class HistoryService
{
    private GameResultRepository _repository;
    private NavigatorService? _navigator;

    public List<GameResult>? GameResults;

    public string UserName { get; set; } = GameResult.DEFAULT_USER_NAME;

    public HistoryService(
        GameResultRepository repository,
        NavigationManager navigationManager = null
    )
    {
        _repository = repository;
        _navigator = new NavigatorService(navigationManager);
    }

    public async Task LoadHistory()
    {
        GameResults = await _repository.GetAll();
    }

    public async void InitSaveGameResult(int quizId, int score)
    {
        await SaveGameResult(quizId, score);
        _navigator.GoToSelectQuiz();
    }

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