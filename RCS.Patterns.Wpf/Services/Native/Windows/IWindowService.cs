using System.Windows;
using System.Windows.Media;

namespace RCS.Services.Native.Windows;

public interface IWindowService
{
	/// <summary>Applies a background blur to the passed window</summary>
	void ApplyBackgroundBlur(Window window, AccentState accentState, uint opacity = 100);

	/// <summary>Hides the passed window from the Windows Task Switcher</summary>
	void HideWindowFromTaskSwitcher(Window window);

	/// <summary>Get the handle of the window with keyboard focus</summary>
	nint GetFocusedWindowHandle();

	/// <summary>Get the details of the display that has the largest intersection with the window with the passed handle</summary>
	MonitorInfoStruct GetMonitorInfoFromWindow(nint hwnd);

	/// <summary>Sets the colour of the window's border</summary>
	void SetBorderColour(Window window, Color color);

	/// <summary>Show's the system menu of a window</summary>
	void ShowSystemMenu(Window window, Point screenPoint);

	/// <summary>Sets the colour of the window's title bar</summary>
	void SetTitleBarColour(Window window, Color color);
}
