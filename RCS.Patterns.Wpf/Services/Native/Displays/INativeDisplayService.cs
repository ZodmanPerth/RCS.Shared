using System;

namespace OKB.Services.Native.Displays;

public interface INativeDisplayService
{
	/// <summary>Get's the handle of the display at the passed coordinates</summary>
	[Obsolete("Use Rect.ClosestContainer instead")]
	IntPtr GetDisplayHandleAt(int left, int top);

	/// <summary>Gets the information about all displays attached to the computer</summary>
	DisplayInfoCollection GetDisplays();

	/// <summary>Returns the work area of the display containing the window with keyboard focus</summary>
	RectStruct GetWorkAreaOfDisplayContainingWindowWithKeyboardFocus();

	/// <summary>Returns the work area of the display containing the mouse pointer</summary>
	RectStruct GetWorkAreaOfDisplayContainingMousePointer();
}