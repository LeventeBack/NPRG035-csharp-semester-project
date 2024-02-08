using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using QuizEdu.Src.Models;
using QuizEdu.Src.Data;

namespace QuizEdu.Src.Services;

/// <summary>
/// This service class is used to manage quiz-related operations.
/// </summary>
public class ManageQuizService
{
    private QuizRepository _repository;
    private NavigatorService _navigator;
    private JsInteractionService _jsInteraction;

    /// <summary>
    /// The list of quizzes which are showed on the UI as a list.
    /// </summary>
    public List<Quiz>? Quizzes;

    /// <summary>
    /// The selected quiz which will be loaded to the UI for editing.
    /// </summary>
    public Quiz? SelectedQuiz;

    /// <summary>
    /// Constructor to initialize the ManageQuizService with the quiz repository, navigation manager, and JavaScript runtime.
    /// </summary>
    /// <param name="repository"></param>
    /// <param name="navigationManager"></param>
    /// <param name="jsRuntime"></param>
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

    /// <summary>
    /// This method is used to load the quizzes from the database.
    /// </summary>
    /// <returns></returns>
    public async Task LoadQuizzes()
    {
        Quizzes = await _repository.Get();
    }

    /// <summary>
    /// This method is used to load a quiz by id from the database. 
    /// If the quiz does not exist, it navigates to the manage quizzes page. 
    /// </summary>
    /// <param name="quizId"></param>
    /// <returns></returns>
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

    /// <summary>
    /// This method is used to navigate to the quiz page to start the quiz.
    /// </summary>
    /// <param name="quiz"></param>
    public void InitStartQuiz(Quiz quiz)
    {
        if (quiz != null)
        {
            _navigator.GoToQuiz(quiz.Id);
        }
    }

    /// <summary>
    /// This method is used to create a new quiz and navigate to the manage questions 
    /// page to add questions to the quiz.
    /// </summary>
    /// <returns></returns>
    public async Task InitCreateQuiz()
    {
        Quiz quiz = new Quiz();
        int quizId = await _repository.Create(quiz);
        _navigator.GoToManageQuestions(quizId);
    }

    /// <summary>
    /// This method is used to create a new combined quiz and navigate to the manage questions
    /// page to select quizzes to use for this quiz.
    /// </summary>
    /// <returns></returns>
    public async Task InitCreateCombinedQuiz()
    {
        Quiz quiz = new Quiz
        {
            Type = 1
        };
        int quizId = await _repository.Create(quiz);
        _navigator.GoToManageQuestions(quizId);
    }

    /// <summary>
    /// This method is used to navigate to the manage questions page to add questions to the quiz.
    /// </summary>
    /// <param name="quiz"></param>
    public void InitEditQuiz(Quiz quiz)
    {
        if (quiz != null)
        {
            _navigator.GoToManageQuestions(quiz.Id);
        }
    }

    /// <summary>
    /// This method is used to initialize the selected quiz for saving.
    /// After saving the quiz, it navigates to the manage quizzes page.
    /// </summary>
    /// <returns></returns>
    public async Task InitSaveQuiz()
    {
        await _repository.Update(SelectedQuiz);
        _navigator.GoToManageQuizzes();
    }

    /// <summary>
    /// This method is used to initialize the selected quiz for deletion.
    /// It shows a confirmation dialog to the user before deleting the quiz.
    /// </summary>
    /// <param name="quiz"></param>
    /// <returns></returns>
    public async Task InitDeleteQuiz(Quiz quiz)
    {
        var confirm = await _jsInteraction.ConfirmDelete(quiz.Title);
        if (confirm)
        {
            await _repository.Delete(quiz.Id);
            Quizzes?.Remove(quiz);
        }
    }

    /// <summary>
    /// This method is used to load the quizzes which are playable from the database.
    /// This condition is based on the number of questions and rounds.
    /// </summary>
    /// <returns></returns>
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

    /// <summary>
    /// This method is used to update the selected quiz with the new question.
    /// </summary>
    /// <param name="question"></param>
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