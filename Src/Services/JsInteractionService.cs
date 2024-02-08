using Microsoft.JSInterop;

namespace QuizEdu.Src.Services;

/// <summary>
/// This class is used to interact with JavaScript code.
/// It provides methods to show a confirmation dialog and play audio.
/// </summary>
public class JsInteractionService
{
    private IJSRuntime? _jsRuntime;

    /// <summary>
    /// Constructor to initialize the JsInteractionService with the JS runtime.
    /// </summary>
    /// <param name="jsRuntime"></param>
    public JsInteractionService(IJSRuntime jsRuntime)
    {
        _jsRuntime = jsRuntime;
    }

    /// <summary>
    /// This method is used to show a confirmation dialog.
    /// </summary>
    /// <param name="message"></param>
    /// <returns>true if the user confirms, false otherwise</returns>
    public async Task<bool> Confirm(string message)
    {
        if (_jsRuntime == null)
        {
            return false;
        }

        return await _jsRuntime.InvokeAsync<bool>("confirm", message);
    }

    /// <summary>
    /// This method is used to show a confirmation dialog for deleting an item.
    /// </summary>
    /// <param name="item"></param>
    /// <returns>true if the user confirms the delete, false otherwise</returns>
    public async Task<bool> ConfirmDelete(string item)
    {
        var confirm = await Confirm($"Are you sure you want to delete {item}?");

        return confirm;
    }

    /// <summary>
    /// This method is used to play audio with the given source.
    /// </summary>
    /// <param name="audioSrc"></param>
    public async Task PlayAudio(string audioSrc)
    {
        if (_jsRuntime == null)
        {
            return;
        }

        await _jsRuntime.InvokeVoidAsync("PlayAudioFile", audioSrc);
    }
}