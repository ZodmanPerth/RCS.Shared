namespace RCS.Services;

public class ClipboardService : IClipboardService
{
	readonly IClipboardHistoryService _clipboardHistoryService;



	//// Lifecycle


	public ClipboardService(IClipboardHistoryService clipboardHistoryService)
	{
		_clipboardHistoryService = clipboardHistoryService ?? throw new ArgumentNullException(nameof(clipboardHistoryService));
	}



	//// Actions


	public bool IsTextOnClipboard(string text)
	{
		if (text.IsNullOrWhitespace())
			return false;

		if (!_clipboardHistoryService.ClipboardItems.Any())
			return false;

		var firstItem = _clipboardHistoryService.ClipboardItems.First();
		if (!firstItem.IsRegularText)
			return false;

		return firstItem.Text == text;
	}
}

