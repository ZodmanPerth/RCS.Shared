using OKB.Services.Native.Windows;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Windows;

#nullable disable

namespace OKB.Services.Native;

[StructLayout(LayoutKind.Sequential)]
record struct AccentPolicyStruct
{
	public AccentState AccentState;
	public uint AccentFlags;
	public uint GradientColor;
	public uint AnimationId;
}

/// <remarks>COPYDATASTRUCT</remarks>
[StructLayout(LayoutKind.Sequential)]
public record struct CopyDataStruct
{
	public IntPtr dwData;
	public int cbData;
	public IntPtr lpData;
}

[StructLayout(LayoutKind.Sequential)]
record struct GuiThreadInfoStruct
{
	public int cbSize;
	public uint flags;
	public IntPtr hwndActive;
	public IntPtr hwndFocus;
	public IntPtr hwndCapture;
	public IntPtr hwndMenuOwner;
	public IntPtr hwndMoveSize;
	public IntPtr hwndCaret;
	public RectStruct rcCaret;
}

/// <remarks>HARDWAREINPUT</remarks>
[StructLayout(LayoutKind.Sequential)]
record struct HardwareInput
{
	public uint uMsg;
	public ushort wParamL;
	public ushort wParamH;
}

/// <remarks>INPUT</remarks>
[StructLayout(LayoutKind.Sequential)]
record struct Input
{
	public int type;
	public InputUnion u;
}

[StructLayout(LayoutKind.Explicit)]
record struct InputUnion
{
	/// <summary>The event is a mouse event. Use the mi structure of the union.</summary>
	[FieldOffset(0)]
	public MouseInput mi;

	/// <summary>The event is a keyboard event. Use the ki structure of the union.</summary>
	[FieldOffset(0)]
	public KeyboardInput ki;

	/// <summary>The event is a hardware event. Use the hi structure of the union.</summary>
	[FieldOffset(0)]
	public HardwareInput hi;
}

/// <remarks>KEYBDINPUT</remarks>
[StructLayout(LayoutKind.Sequential)]
record struct KeyboardInput
{
	/// <summary>Virtual keycode [1-254]</summary>
	/// <remarks>Must be 0 when dwFlags is <see cref="InputDwFlags.KEYEVENTF_UNICODE"/></remarks>
	public ushort wVk;

	/// <summary>A hardware scan code for the key</summary>
	/// <remarks>Specifies a unicode character when dwFlags is <see cref="InputDwFlags.KEYEVENTF_UNICODE"/></remarks>
	public ushort wScan;

	/// <summary>Specifies various aspects of a keystroke.  See KEYEVENTF_ in <see cref="InputDwFlags"/>.</summary>
	public uint dwFlags;

	/// <summary>The time stamp of the event in milliseconds</summary>
	/// <remarks>When zero the system provides the timestamp</remarks>
	public uint time;

	/// <summary>Use the user32.GetMessageExtraInfo win32 function to obtain this information/// </summary>
	public IntPtr dwExtraInfo;
}

/// <remarks>MOUSEINPUT</remarks>
[StructLayout(LayoutKind.Sequential)]
record struct MouseInput
{
	public int dx;
	public int dy;
	public uint mouseData;
	public uint dwFlags;
	public uint time;
	public IntPtr dwExtraInfo;
}

[DebuggerDisplay("({X},{Y})")]
[StructLayout(LayoutKind.Sequential)]
public record struct PointStruct
{
	public int X;
	public int Y;

	public PointStruct(int x, int y)
	{
		X = x;
		Y = y;
	}
}

[StructLayout(LayoutKind.Sequential)]
public record struct MonitorInfoStruct
{
	/// <summary>
	/// The size, in bytes, of the structure. Set this member to sizeof(MONITORINFOEX) (72) before calling the GetMonitorInfo function.
	/// Doing so lets the function determine the type of structure you are passing to it.
	/// </summary>
	public int Size;

	/// <summary>
	/// A RECT structure that specifies the display monitor rectangle, expressed in virtual-screen coordinates.
	/// Note that if the monitor is not the primary display monitor, some of the rectangle's coordinates may be negative values.
	/// </summary>
	public RectStruct Monitor;

	/// <summary>
	/// A RECT structure that specifies the work area rectangle of the display monitor that can be used by applications,
	/// expressed in virtual-screen coordinates. Windows uses this rectangle to maximize an application on the monitor.
	/// The rest of the area in Monitor contains system windows such as the task bar and side bars.
	/// Note that if the monitor is not the primary display monitor, some of the rectangle's coordinates may be negative values.
	/// </summary>
	public RectStruct WorkArea;

	/// <summary>
	/// The attributes of the display monitor.
	///
	/// This member can be the following value:
	///   1 : MONITORINFOF_PRIMARY
	/// </summary>
	public uint Flags;
}

[StructLayout(LayoutKind.Sequential)]
public record struct RectStruct
{
	public int Left;
	public int Top;
	public int Right;
	public int Bottom;

	public RectStruct(int left, int top, int right, int bottom)
	{
		Left = left;
		Top = top;
		Right = right;
		Bottom = bottom;
	}

	public override string ToString() =>
		$"({Left},{Top},{Right},{Bottom})";

	public override int GetHashCode() =>
		HashCode.Combine(Left, Top, Right, Bottom);

	public Rect ToRect() =>
		new(Left, Top, Right - Left, Bottom - Top);
}

[StructLayout(LayoutKind.Sequential)]
public struct WindowCompositionAttributeDataStruct
{
	public WindowCompositionAttributeStruct Attribute;
	public IntPtr Data;
	public int SizeOfData;
}
