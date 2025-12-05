namespace OKB.Services;

public interface IClipboardService
{
	/// <summary>true if the current clipboard item is the passed text</summary>
	bool IsTextOnClipboard(string text);
}