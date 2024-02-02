using QuizEdu.Src.Models;
using MudBlazor;

namespace QuizEdu.Src.Services;


public class QuizGamePlayService
{
    public const int NEXT_QUESTION_DELAY = 4000;
    public const int OPTION_COUNT = 4;
    private Quiz _quiz { get; set; }
    private int _score { get; set; } = 0;
    private int _currentIndex { get; set; } = 0;
    private bool _hasEnded { get; set; } = false;
    private Option _selectedOption { get; set; }

    public bool HasAnswered => _selectedOption != null;
    public bool HasEnded => _hasEnded;
    public bool IsCorrect => _selectedOption.IsCorrect;
    public int Score => _score;
    public int CurrentRound => _currentIndex + 1;

    public QuizGamePlayService(Quiz activeQuiz)
    {
        _quiz = activeQuiz;
        Shuffle(_quiz.Questions);
        ShuffleOptions();
    }

    public Question GetCurrentQuestion()
    {
        if (_currentIndex < _quiz.Questions.Count)
        {
            return _quiz.Questions[_currentIndex];
        }
        return null;
    }

    public Color GetOptionColor(Option option)
    {

        if (!HasAnswered) return Color.Primary;

        if (option.IsCorrect) return Color.Success;

        if (_selectedOption == option) return Color.Error;

        return Color.Primary;
    }

    public async Task HandleOptionClick(Option option)
    {
        if (HasAnswered) return;

        _selectedOption = option;

        if (option.IsCorrect) _score++;
    }

    public void NextQuestion()
    {
        if (_currentIndex + 1 >= _quiz.Questions.Count)
        {
            _hasEnded = true;
            return;
        }

        _currentIndex++;
        _selectedOption = null;
    }

    private void ShuffleOptions()
    {
        foreach (var question in _quiz.Questions)
        {
            Shuffle(question.Options);
        }
    }

    private void Shuffle<T>(List<T> list)
    {
        Random rng = new Random();
        int n = list.Count;
        while (n > 1)
        {
            n--;
            int k = rng.Next(n + 1);
            T value = list[k];
            list[k] = list[n];
            list[n] = value;
        }
    }

}