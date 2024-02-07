using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using QuizEdu.Src.Models;
using QuizEdu.Src.Data;

namespace QuizEdu.Src.Services;

public class ManageQuestionService
{
    private QuestionRepository _repository;
    private NavigatorService _navigator;
    private JsInteractionService _jsInteraction;

    public Question SelectedQuestion;

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

    public void InitEditQuestion(Question question)
    {
        question.Options = question.Options.OrderBy(o => o.IsCorrect ? 0 : 1).ToList();
        SelectedQuestion = question;
    }

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