using Microsoft.UI.Input;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows;
using System.Windows.Interop;
using System.Windows.Media;

namespace RCS.Services.Native.Windows
{
	public class WindowService : IWindowService
	{
		const uint BlurBackgroundColor = 0x990000;
		const uint WhiteMask = 0xFFFFFF;



		//// Helpers


		int IntPtrToInt32(IntPtr intPtr) =>
			unchecked((int)intPtr.ToInt64());

		/// <summary>Set a property on the window with the passed handle</summary>
		IntPtr SetWindowLong(IntPtr hWnd, int nIndex, IntPtr dwNewLong)
		{
			int error = 0;
			IntPtr result = IntPtr.Zero;

			// Win32 SetWindowLong doesn't clear error on success
			SetLastError(0);

			if (IntPtr.Size == 4)
			{
				// use SetWindowLong
				Int32 tempResult = IntSetWindowLong(hWnd, nIndex, IntPtrToInt32(dwNewLong));
				error = Marshal.GetLastWin32Error();
				result = new IntPtr(tempResult);
			}
			else
			{
				// use SetWindowLongPtr
				result = IntSetWindowLongPtr(hWnd, nIndex, dwNewLong);
				error = Marshal.GetLastWin32Error();
			}

			if ((result == IntPtr.Zero) && (error != 0))
				throw new System.ComponentModel.Win32Exception(error);

			return result;
		}


		/// <summary>Get the text from the title bar of the window with the passed handle</summary>
		string GetWindowTitle(IntPtr hwnd)
		{
			int length = GetWindowTextLength(hwnd);
			if (length == 0) return string.Empty;

			StringBuilder sb = new StringBuilder(length + 1);
			GetWindowText(hwnd, sb, sb.Capacity);
			return sb.ToString();
		}



		//// Actions


		// Kudos https://github.com/jdscodelab/LoginUIBlurredAcrylicBackground
		public void ApplyBackgroundBlur(Window window, AccentState accentState, uint opacity = 100)
		{
			var windowHelper = new WindowInteropHelper(window);

			var accent = new AccentPolicyStruct();
			accent.AccentState = accentState;
			accent.GradientColor = (opacity << 24) | (BlurBackgroundColor & WhiteMask);

			var accentStructSize = Marshal.SizeOf(accent);
			var accentPtr = Marshal.AllocHGlobal(accentStructSize);
			Marshal.StructureToPtr(accent, accentPtr, false);

			var data = new WindowCompositionAttributeDataStruct();
			data.Attribute = WindowCompositionAttributeStruct.WCA_ACCENT_POLICY;
			data.SizeOfData = accentStructSize;
			data.Data = accentPtr;

			SetWindowCompositionAttribute(windowHelper.Handle, ref data);

			Marshal.FreeHGlobal(accentPtr);
		}

		// Kudos https://stackoverflow.com/a/551847/117797
		public void HideWindowFromTaskSwitcher(Window window)
		{
			WindowInteropHelper wndHelper = new WindowInteropHelper(window);

			int exStyle = (int)GetWindowLong(wndHelper.Handle, (int)GetWindowLongFields.GWL_EXSTYLE);

			exStyle |= (int)WindowStylesEx.WS_EX_TOOLWINDOW;
			SetWindowLong(wndHelper.Handle, (int)GetWindowLongFields.GWL_EXSTYLE, (IntPtr)exStyle);
		}

		public IntPtr GetFocusedWindowHandle()
		{
			IntPtr foregroundWindow = GetForegroundWindow();
			uint threadId = GetWindowThreadProcessId(foregroundWindow, out _);

			GuiThreadInfoStruct info = new GuiThreadInfoStruct();
			info.cbSize = Marshal.SizeOf(info);

			if (GetGUIThreadInfo(threadId, ref info))
				return info.hwndFocus;

			return IntPtr.Zero;
		}

		public MonitorInfoStruct GetMonitorInfoFromWindow(IntPtr hwnd)
		{
			IntPtr hMonitor = MonitorFromWindow(hwnd, MonitorOptions.MONITOR_DEFAULTTONEAREST);

			var info = new MonitorInfoStruct();
			info.Size = Marshal.SizeOf(info);

			if (!GetMonitorInfo(hMonitor, ref info))
				return default;

			return info;
		}

		public void SetTitleBarColour(Window window, Color color)
		{
			var hwnd = new WindowInteropHelper(window).Handle;
			uint colorRef = ((uint)color.R) | ((uint)color.G << 8) | ((uint)color.B << 16);
			DwmSetWindowAttribute(hwnd, DesktopWindowModuleWindowAttributes.TitleBarColour, ref colorRef, sizeof(uint));
		}

		public void SetBorderColour(Window window, Color color)
		{
			var hwnd = new WindowInteropHelper(window).Handle;
			uint colorRef = ((uint)color.R) | ((uint)color.G << 8) | ((uint)color.B << 16);
			DwmSetWindowAttribute(hwnd, DesktopWindowModuleWindowAttributes.BorderColour, ref colorRef, sizeof(uint));
		}

		public void ShowSystemMenu(Window window, Point screenPoint)
		{
			var hwnd = new WindowInteropHelper(window).Handle;
			var hMenu = GetSystemMenu(hwnd, false);

			var cmd = TrackPopupMenu(hMenu, (uint)(Native.TrackPopupMenu.LeftAlign | Native.TrackPopupMenu.ReturnCommand), (int)screenPoint.X, (int)screenPoint.Y, 0, hwnd, nint.Zero);
			if (cmd != 0)
				SendMessage(hwnd, (uint)WindowsMessage.SystemCommand, (IntPtr)cmd, IntPtr.Zero);
		}



		//// Interop


		[DllImport("dwmapi.dll")]
		static extern int DwmSetWindowAttribute(IntPtr hwnd, DesktopWindowModuleWindowAttributes attr, ref uint attrValue, int attrSize);

		[DllImport("user32.dll")]
		static extern IntPtr GetWindowLong(IntPtr hWnd, int nIndex);

		[DllImport("user32.dll")]
		static extern bool GetGUIThreadInfo(uint idThread, ref GuiThreadInfoStruct lpgui);

		[DllImport("user32.dll")]
		static extern IntPtr GetForegroundWindow();

		[DllImport("user32.dll")]
		static extern bool GetMonitorInfo(IntPtr hMonitor, ref MonitorInfoStruct lpmi);

		[DllImport("user32.dll")]
		static extern IntPtr GetSystemMenu(IntPtr hWnd, bool bRevert);

		[DllImport("user32.dll", SetLastError = true, CharSet = CharSet.Auto)]
		static extern int GetWindowText(IntPtr hWnd, StringBuilder lpString, int nMaxCount);

		[DllImport("user32.dll", SetLastError = true)]
		static extern int GetWindowTextLength(IntPtr hWnd);

		[DllImport("user32.dll")]
		static extern uint GetWindowThreadProcessId(IntPtr hWnd, out uint lpdwProcessId);

		[DllImport("user32.dll", EntryPoint = "SetWindowLong", SetLastError = true)]
		static extern Int32 IntSetWindowLong(IntPtr hWnd, int nIndex, Int32 dwNewLong);

		[DllImport("user32.dll", EntryPoint = "SetWindowLongPtr", SetLastError = true)]
		static extern IntPtr IntSetWindowLongPtr(IntPtr hWnd, int nIndex, IntPtr dwNewLong);

		[DllImport("user32.dll")]
		static extern IntPtr MonitorFromWindow(IntPtr hwnd, MonitorOptions dwFlags);

		[DllImport("user32.dll")]
		static extern IntPtr SendMessage(IntPtr hWnd, uint Msg, IntPtr wParam, IntPtr lParam);

		[DllImport("kernel32.dll", EntryPoint = "SetLastError")]
		static extern void SetLastError(int dwErrorCode);

		[DllImport("user32.dll")]
		static extern int SetWindowCompositionAttribute(IntPtr hwnd, ref WindowCompositionAttributeDataStruct data);

		[DllImport("user32.dll")]
		static extern int TrackPopupMenu(IntPtr hMenu, uint uFlags, int x, int y, int nReserved, IntPtr hWnd, IntPtr prcRect);
	}
}
