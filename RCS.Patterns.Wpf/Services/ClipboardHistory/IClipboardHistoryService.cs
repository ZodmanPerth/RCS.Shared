using RCS.Models;
using RCS.Patterns;

namespace RCS.Services;

public interface IClipboardHistoryService
{
	/// <summary>The clipboard history</summary>
	ObservableCollectionEx<ClipboardItem> ClipboardItems { get; }
}