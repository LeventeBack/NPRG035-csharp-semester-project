using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using QuizEdu.Src.Models;
using QuizEdu.Src.Data;

namespace QuizEdu.Src.Services;

public class ManageQuizService
{
    private QuizRepository _repository;
    private NavigatorService _navigator;
    private JsInteractionService _jsInteraction;

    public List<Quiz>? Quizzes;

    public Quiz? SelectedQuiz;

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

    public async Task LoadQuiz(int quizId)
    {
        if (quizId != 0)
        {
            SelectedQuiz = await _repository.Get(quizId);
        }
        else
        {
            _navigator.GoToManageQuizzes();
        }
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
        Quiz quiz = new Quiz();
        int quizId = await _repository.Create(quiz);
        _navigator.GoToManageQuestions(quizId);
    }

    public async Task InitCreateCombinedQuiz()
    {
        Quiz quiz = new Quiz
        {
            Type = 1
        };
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

    public async Task InitSaveQuiz()
    {
        await _repository.Update(SelectedQuiz);
        _navigator.GoToManageQuizzes();
    }

    public async Task InitDeleteQuiz(Quiz quiz)
    {
        var confirm = await _jsInteraction.ConfirmDelete(quiz.Title);
        if (confirm)
        {
            await _repository.Delete(quiz.Id);
            Quizzes?.Remove(quiz);
        }
    }

    public async Task LoadPlayableQuizzes()
    {
        await LoadQuizzes();
        List<Quiz> playableQuizzes = new List<Quiz>();
        foreach (var quiz in Quizzes)
        {
            if (await _repository.IsQuizPlayable(quiz.Id))
            {
                playableQuizzes.Add(quiz);
            }
        }
        Quizzes = playableQuizzes;
    }

    public void UpdateSelectedQuiz(Question question)
    {
        if (SelectedQuiz == null || SelectedQuiz.Questions == null)
        {
            return;
        }
        if (SelectedQuiz.IsCombinedQuiz())
        {
            return;
        }

        int index = SelectedQuiz.Questions.FindIndex(q => q.Id == question.Id);
        if (index != -1)
        {
            SelectedQuiz.Questions[index] = question;
        }
        else
        {
            SelectedQuiz.Questions.Add(question);
        }
    }
}