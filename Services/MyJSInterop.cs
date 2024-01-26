using Microsoft.JSInterop;

namespace DevBuddy.Services;

public class MyJSInterop
{
    private readonly IJSRuntime _jsRuntime;

    public MyJSInterop(IJSRuntime jsRuntime)
    {
        _jsRuntime = jsRuntime;
    }

    public ValueTask OpenURL(string url, string target = "_blank") => _jsRuntime.InvokeVoidAsync("open", url, target);
}