using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using QuizEdu.Src.Models;
using QuizEdu.Src.Data;

namespace QuizEdu.Src.Services;

/// <summary>
/// This service class is used to manage question-related operations.
/// </summary>
public class ManageQuestionService
{
    private QuestionRepository _repository;
    private NavigatorService _navigator;
    private JsInteractionService _jsInteraction;

    /// <summary>
    /// The selected question. This question's values are loaded to the UI for editing.
    /// </summary>
    public Question SelectedQuestion;

    /// <summary>
    /// Constructor to initialize the ManageQuestionService with the question repository, navigation manager, and JavaScript runtime.
    /// </summary>
    /// <param name="repository"></param>
    /// <param name="navigationManager"></param>
    /// <param name="jsRuntime"></param>
    public ManageQuestionService(
        QuestionRepository repository,
        NavigationManager navigationManager,
        IJSRuntime jsRuntime = null
    )
    {
        _repository = repository;
        _navigator = new NavigatorService(navigationManager);
        _jsInteraction = new JsInteractionService(jsRuntime);
        SelectedQuestion = new Question();
    }

    /// <summary>
    /// This method is used to initialize the selected question for editing.
    /// It sorts the options, so that the correct option is always the first option.
    /// </summary>
    /// <param name="question"></param>
    public void InitEditQuestion(Question question)
    {
        question.Options = question.Options.OrderBy(o => o.IsCorrect ? 0 : 1).ToList();
        SelectedQuestion = question;
    }

    /// <summary>
    /// This method is used to initialize the selected question for saving.
    /// Sets the correct option to the first option and saves the question to the database.
    /// After saving the question, it resets the selected question to a new empty question.
    /// </summary>
    /// <param name="quizId"></param>
    public async void InitSaveQuestion(int quizId)
    {
        SelectedQuestion.Options.ForEach(o => o.IsCorrect = false);
        SelectedQuestion.Options[0].IsCorrect = true;
        if (SelectedQuestion.Id == 0)
        {
            SelectedQuestion.QuizId = quizId;
            await _repository.Create(SelectedQuestion);
        }
        else
        {
            await _repository.Update(SelectedQuestion);
        }
        SelectedQuestion = new Question();
    }

    /// <summary>
    /// This method is used to initialize the selected question for deletion.
    /// It shows a confirmation dialog to the user before deleting the question.
    /// </summary>
    /// <param name="question"></param>
    public async void InitDeleteQuestion(Question question)
    {
        var confirm = await _jsInteraction.ConfirmDelete(question.Text);

        if (confirm)
        {
            await _repository.Delete(question);
            _navigator.GoToManageQuestions(question.QuizId);
        }

    }
}