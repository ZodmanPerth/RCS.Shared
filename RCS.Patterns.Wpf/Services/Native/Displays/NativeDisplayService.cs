using OKB.Services.Native.Windows;
using System.Runtime.InteropServices;

namespace OKB.Services.Native.Displays;

public class NativeDisplayService : INativeDisplayService
{
	const uint MONITORINFOF_PRIMARY = 1;

	readonly IWindowService _windowService;



	//// Lifecycle


	public NativeDisplayService(IWindowService windowService)
	{
		_windowService = windowService ?? throw new ArgumentNullException(nameof(windowService));
	}



	//// Helpers


	MonitorInfoStruct GetDisplayInfoAtPoint(PointStruct point)
	{
		var monitorHandle = MonitorFromPoint(point, MonitorOptions.MONITOR_DEFAULTTONEAREST);

		var info = new MonitorInfoStruct();
		info.Size = Marshal.SizeOf(info);

		if (!GetMonitorInfo(monitorHandle, ref info))
			return default;

		return info;
	}



	//// Actions


	public IntPtr GetDisplayHandleAt(int left, int top) =>
		MonitorFromPoint(new PointStruct(left, top), MonitorOptions.MONITOR_DEFAULTTONEAREST);

	public DisplayInfoCollection GetDisplays()
	{
		DisplayInfoCollection col = new DisplayInfoCollection();

		EnumDisplayMonitors
		(
			IntPtr.Zero,
			IntPtr.Zero,
			delegate (IntPtr hMonitor, IntPtr hdcMonitor, ref RectStruct lprcMonitor, IntPtr dwData)
			{
				var mi = new MonitorInfoStruct();
				mi.Size = Marshal.SizeOf(mi);
				bool success = GetMonitorInfo(hMonitor, ref mi);
				if (success)
				{
					DisplayInfo di = new DisplayInfo();
					di.Handle = hMonitor;
					di.Bounds = mi.Monitor.ToRect();
					di.WorkBounds = mi.WorkArea.ToRect();
					di.IsPrimary = (mi.Flags & MONITORINFOF_PRIMARY) > 0;
					col.Add(di);
				}
				return true;
			},
			IntPtr.Zero
		);
		return col;
	}

	public RectStruct GetWorkAreaOfDisplayContainingWindowWithKeyboardFocus()
	{
		var activeWindowHandle = _windowService.GetFocusedWindowHandle();
		if (activeWindowHandle == IntPtr.Zero)
			return default;

		var monitorInfo = _windowService.GetMonitorInfoFromWindow(activeWindowHandle);
		if (monitorInfo.WorkArea == default)
			return default;

		return monitorInfo.WorkArea;
	}

	public RectStruct GetWorkAreaOfDisplayContainingMousePointer()
	{
		GetCursorPos(out PointStruct cursorPosition);
		var monitorInfo = GetDisplayInfoAtPoint(cursorPosition);
		if (monitorInfo.WorkArea == default)
			return default;

		return monitorInfo.WorkArea;
	}



	//// Interop


	delegate bool MonitorEnumProc(IntPtr hMonitor, IntPtr hdcMonitor, ref RectStruct lprcMonitor, IntPtr dwData);


	[DllImport("user32.dll")]
	static extern bool EnumDisplayMonitors(IntPtr hdc, IntPtr lprcClip, MonitorEnumProc lpfnEnum, IntPtr dwData);

	[DllImport("user32.dll")]
	public static extern bool GetCursorPos(out PointStruct lpPoint);

	[DllImport("User32.dll")]
	static extern bool GetMonitorInfo(IntPtr hMonitor, ref MonitorInfoStruct lpmi);

	[DllImport("user32.dll", SetLastError = true)]
	static extern IntPtr MonitorFromPoint(PointStruct pt, MonitorOptions dwFlags);


}
