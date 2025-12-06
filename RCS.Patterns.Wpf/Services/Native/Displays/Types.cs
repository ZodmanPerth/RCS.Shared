using System.Windows;

namespace RCS.Services.Native.Displays;

public record struct DisplayInfo
{
	/// <summary>Handle of the display</summary>
	public IntPtr Handle { get; set; }

	/// <summary>True when the display is the primary display</summary>
	public bool IsPrimary { get; set; }

	/// <summary>The height of the Display</summary>
	public readonly double Height =>
		Bounds.Height;

	/// <summary>The width of the display</summary>
	public readonly double Width =>
		Bounds.Width;

	/// <summary>The bounds of the display</summary>
	public Rect Bounds { get; set; }

	/// <summary>The work area of the diplsy</summary>
	/// <remarks>i.e. accounts for docked Windows taskbar</remarks>
	public Rect WorkBounds { get; set; }

	public readonly Rect AsRect() =>
		new Rect(WorkBounds.X, WorkBounds.Y, Width, Height);

	public override readonly string ToString() =>
		$"0x{Handle:x16} {(IsPrimary ? "Primary " : null)}({Bounds}){(WorkBounds != Bounds ? $" ({WorkBounds})" : null)}";
}

public class DisplayInfoCollection : List<DisplayInfo> { /* nada */ }
