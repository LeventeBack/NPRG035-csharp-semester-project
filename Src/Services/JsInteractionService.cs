using Microsoft.JSInterop;

namespace QuizEdu.Src.Services;

public class JsInteractionService
{
    private IJSRuntime? _jsRuntime;

    public JsInteractionService(IJSRuntime jsRuntime)
    {
        _jsRuntime = jsRuntime;
    }

    public async Task<bool> Confirm(string message)
    {
        if (_jsRuntime == null)
        {
            return false;
        }

        return await _jsRuntime.InvokeAsync<bool>("confirm", message);
    }

    public async Task<bool> ConfirmDelete(string item)
    {
        var confirm = await Confirm($"Are you sure you want to delete {item}?");

        return confirm;
    }
}