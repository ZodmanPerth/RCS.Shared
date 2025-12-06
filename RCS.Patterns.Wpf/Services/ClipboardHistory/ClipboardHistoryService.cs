using OKB.Logging;
using OKB.Models;
using RCS.Patterns;
using OKB.Utilities;
using CommunityToolkit.Mvvm.ComponentModel;
using Windows.ApplicationModel.DataTransfer;

namespace OKB.Services;

public partial class ClipboardHistoryService : ObservableObject, IClipboardHistoryService
{
	public ObservableCollectionEx<ClipboardItem> ClipboardItems { get; private set; }

	readonly ThrottledAction _readClipboardThrottledAction;



	//// Lifecycle


	public ClipboardHistoryService(ThrottledActionFactory throttledActionFactory)
	{
		if (throttledActionFactory is null) throw new ArgumentNullException(nameof(throttledActionFactory));

		ClipboardItems = new();


		// Create throttled action for reading the Clipboard.
		// This prevents thrashing the observable collection due to the clipboard changing
		// multiple times when the the clipboard is being manipulated.
		_readClipboardThrottledAction = throttledActionFactory.CreateToRunOnBackgroundThreads
		(
			SynchroniseAvailableClipboardItems,
			TimeSpan.FromMilliseconds(100)
		);


		Clipboard.HistoryChanged += (s, e) => _readClipboardThrottledAction.Invoke();


		SynchroniseAvailableClipboardItems();


		return;


		//// Local Functions


		async void SynchroniseAvailableClipboardItems()
		{
			try
			{
				var history = await Clipboard.GetHistoryItemsAsync();

				var index = 0;
				var items = new List<ClipboardItem>();
				foreach (var item in history.Items)
				{
					var content = item.Content;
					if (!content.Contains(StandardDataFormats.Text))
						continue;

					var text = await content.GetTextAsync();

					items.Add(new ClipboardItem(text, index));

					index++;
				}

				ClipboardItems.ResetContent(items);
			}
			catch (Exception ex)
			{
				Logger.AsError("Couldn't get windows clipboard content")
					.WithException(ex)
					.Write();

				return;
			}
		}
	}
}
