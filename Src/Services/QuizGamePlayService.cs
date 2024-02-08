using QuizEdu.Src.Models;
using MudBlazor;
using Microsoft.JSInterop;
using QuizEdu.Src.Data;

namespace QuizEdu.Src.Services;

/// <summary>
/// This class is used to orchestrate the quiz game play.
/// It contains several methods to handle the game play logic.
/// It also contains constants for certain game play settings.
/// </summary>
public class QuizGamePlayService
{
    /// <summary>
    /// The path to the correct sound's audio file
    /// </summary>
    public const string CORRECT_AUDIO = "/sounds/correct.wav";

    /// <summary>
    /// The path to the wrong sound's audio file
    /// </summary>
    public const string WRONG_AUDIO = "/sounds/wrong.wav";

    /// <summary>
    /// The delay in milliseconds before moving to the next question
    /// </summary>
    public const int NEXT_QUESTION_DELAY = 4000;

    /// <summary>
    /// The number of options for each question
    /// </summary>
    public const int OPTION_COUNT = 4;

    /// <summary>
    /// The active quiz used for the game
    /// </summary>
    private Quiz _quiz { get; set; }

    /// <summary>
    /// The list of questions for the game, if the quiz is a normal quiz 
    /// it corresponds to the questions of the quiz, if the quiz is a combined quiz
    /// it corresponds to the questions of the questions of the combined quizzes
    /// </summary>
    private List<Question> _questions { get; set; }

    /// <summary>
    /// The score of the user, so the number of correct answers
    /// </summary>
    private int _score { get; set; } = 0;

    /// <summary>
    /// The maximum number of rounds for the game
    /// </summary>
    private int _roundCount { get; set; }

    /// <summary>
    /// The current index of the question list
    /// </summary>
    private int _currentIndex { get; set; } = 0;

    /// <summary>
    /// The value to store if the game has ended
    /// </summary>
    private bool _hasEnded { get; set; } = false;

    /// <summary>
    /// The selected option by the user
    /// </summary>
    private Option? _selectedOption { get; set; }

    /// <summary>
    /// The JS interaction service to play audio
    /// </summary>
    private JsInteractionService _jsInteraction;

    /// <summary>
    /// Constructor to initialize the QuizGamePlayService with the active quiz, questions, and JS runtime.
    /// </summary>
    /// <param name="activeQuiz"></param>
    /// <param name="questions"></param>
    /// <param name="jSRuntime"></param>
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

    /// <summary>
    /// This method is used to get the current question if the game has more questions.
    /// </summary>
    /// <returns></returns>
    public Question GetCurrentQuestion()
    {
        if (_questions is not null && _currentIndex < _questions.Count)
        {
            return _questions[_currentIndex];
        }
        return null;
    }

    /// <summary>
    /// This method is used to get the color of an option based on the game state.
    /// </summary>
    /// <param name="option"></param>
    /// <returns>The color of the option</returns>
    public Color GetOptionColor(Option option)
    {

        if (!HasAnswered) return Color.Primary;

        if (option.IsCorrect) return Color.Success;

        if (_selectedOption == option) return Color.Error;

        return Color.Primary;
    }

    /// <summary>
    /// This method is used to handle the click event of an option.
    /// It plays the correct or wrong sound based on the selected option.
    /// It also updates the score if the selected option is correct.
    /// </summary>
    /// <param name="option"></param>
    /// <returns></returns>
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

    /// <summary>
    /// This method is used to move to the next question.
    /// It checks if the game has more questions and rounds.
    /// It also resets the selected option.
    /// </summary>
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

    /// <summary>
    /// This method is used to determine the number of rounds for the game.
    /// </summary>
    /// <returns>The number of rounds for the game</returns>
    private int GetRoundCount()
    {
        if (_quiz.RoundCount > 0)
        {
            return Math.Min(_quiz.RoundCount, _questions.Count);
        }
        return _questions.Count;
    }

    /// <summary>
    /// This method is used to shuffle the options for each question.
    /// </summary>
    private void ShuffleOptions()
    {
        foreach (var question in _questions)
        {
            Shuffle(question.Options);
        }
    }

    /// <summary>
    /// This method is used to shuffle a list with any type of elements.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="list"></param>
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

    /// <summary>
    /// This method is used to check if the game has more questions.
    /// </summary>
    /// <returns>true if the game has more questions, false otherwise</returns>
    private bool HasNextQuestion()
    {
        return _currentIndex + 1 < _questions.Count;
    }

    /// <summary>
    /// This method is used to check if the game has ended.
    /// </summary>
    /// <returns>true if the game has ended, false otherwise</returns>
    private bool HasNextRound()
    {
        return _currentIndex + 1 < _roundCount;
    }

    /// <summary>
    /// This method is used to check if the user has answered the current question.
    /// </summary>
    /// <returns>true if the user has answered the question, false otherwise</returns>
    public bool HasAnswered => _selectedOption != null;

    /// <summary>
    /// This method is used to check if the game has ended.
    /// </summary>
    /// <returns>true if the game has ended, false otherwise</returns>
    public bool HasEnded => _hasEnded;

    /// <summary>
    /// This method is used to check if the selected option is correct.
    /// </summary>
    /// <returns>true if the selected option is correct, false otherwise</returns>
    public bool IsCorrect => _selectedOption.IsCorrect;

    /// <summary>
    /// This method is used to get the score of the user.
    /// </summary>
    /// <returns>The score of the user</returns>
    public int Score => _score;

    /// <summary>
    /// This method is used to get the current round of the game.
    /// </summary>
    /// <returns>The current round of the game</returns>
    public int CurrentRound => _currentIndex + 1;
}