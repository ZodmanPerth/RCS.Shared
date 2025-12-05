using OKB.Models;
using OKB.Patterns;

namespace OKB.Services;

public interface IClipboardHistoryService
{
	/// <summary>The clipboard history</summary>
	ObservableCollectionEx<ClipboardItem> ClipboardItems { get; }
}