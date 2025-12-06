using OKB.Models;
using RCS.Patterns;

namespace OKB.Services;

public interface IClipboardHistoryService
{
	/// <summary>The clipboard history</summary>
	ObservableCollectionEx<ClipboardItem> ClipboardItems { get; }
}