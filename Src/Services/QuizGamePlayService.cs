using QuizEdu.Src.Models;
using MudBlazor;
using Microsoft.JSInterop;
using QuizEdu.Src.Data;

namespace QuizEdu.Src.Services;

public class QuizGamePlayService
{
    public const string CORRECT_AUDIO = "/sounds/correct.wav";
    public const string WRONG_AUDIO = "/sounds/wrong.wav";
    public const int NEXT_QUESTION_DELAY = 4000;
    public const int OPTION_COUNT = 4;

    private Quiz _quiz { get; set; }
    private List<Question> _questions { get; set; }
    private int _score { get; set; } = 0;
    private int _roundCount { get; set; }
    private int _currentIndex { get; set; } = 0;
    private bool _hasEnded { get; set; } = false;
    private Option? _selectedOption { get; set; }
    private JsInteractionService _jsInteraction;
    public QuizGamePlayService(
        Quiz activeQuiz,
        List<Question> questions,
        IJSRuntime jSRuntime)
    {
        _quiz = activeQuiz;
        _questions = questions;
        _jsInteraction = new JsInteractionService(jSRuntime);
        Shuffle(_questions);
        ShuffleOptions();
        _roundCount = GetRoundCount();
    }

    public Question GetCurrentQuestion()
    {
        if (_questions is not null && _currentIndex < _questions.Count)
        {
            return _questions[_currentIndex];
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

        string audioSrc = option.IsCorrect ? CORRECT_AUDIO : WRONG_AUDIO;
        await _jsInteraction.PlayAudio(audioSrc);


        if (option.IsCorrect) _score++;

        await Task.Delay(NEXT_QUESTION_DELAY);
        NextQuestion();
    }

    public void NextQuestion()
    {
        if (!HasNextQuestion() || !HasNextRound())
        {
            _hasEnded = true;
            return;
        }

        _currentIndex++;
        _selectedOption = null;
    }

    private int GetRoundCount()
    {
        if (_quiz.RoundCount > 0)
        {
            return Math.Min(_quiz.RoundCount, _questions.Count);
        }
        return _questions.Count;
    }

    private void ShuffleOptions()
    {
        foreach (var question in _questions)
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

    private bool HasNextQuestion()
    {
        return _currentIndex + 1 < _questions.Count;
    }

    private bool HasNextRound()
    {
        return _currentIndex + 1 < _roundCount;
    }

    public bool HasAnswered => _selectedOption != null;
    public bool HasEnded => _hasEnded;
    public bool IsCorrect => _selectedOption.IsCorrect;
    public int Score => _score;
    public int CurrentRound => _currentIndex + 1;
}