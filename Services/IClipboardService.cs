namespace DevBuddy.Services;

public interface IClipboardService
{
    Task CopyToClipboard(string text);
}
