using Microsoft.Xaml.Behaviors;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;

namespace RCS.Controls.Behaviours;

/// <summary>Adds horizontal scroll behaviour using SHIFT + MOUSEWHEEL and WM_MOUSEHWHEEL events (e.g. from a dial)</summary>
public class ScrollViewerHorizontalScrollingBehaviour : Behavior<ScrollViewer>
{
	ScrollViewer? _scrollViewer;



	//// Overrides


	protected override void OnAttached()
	{
		base.OnAttached();

		_scrollViewer = AssociatedObject;
		if (_scrollViewer is null)
			return;

		_scrollViewer.PreviewMouseWheel += OnScrollViewerPreviewMouseWheel;

		_scrollViewer.Loaded += OnLoaded;
		_scrollViewer.Unloaded += OnUnloaded;
	}

	protected override void OnDetaching()
	{
		base.OnDetaching();

		if (_scrollViewer is null)
			return;

		_scrollViewer.PreviewMouseWheel -= OnScrollViewerPreviewMouseWheel;

		_scrollViewer.Loaded -= OnLoaded;
		_scrollViewer.Unloaded -= OnUnloaded;
	}



	//// Helpers


	/// <summary>Handles Shift+MouseWheel for horizontal scrolling</summary>
	void ProcessShiftMouseWheel(object sender, MouseWheelEventArgs e)
	{
		if (_scrollViewer is null) 
			return;

		if (!ReferenceEquals(sender, _scrollViewer))
			return;

		if (Keyboard.Modifiers != ModifierKeys.Shift)
			return;

		var mouseOverScrollViewer = GetLowestHorizontallyScrollableScrollViewerMouseIsOver();
		if (mouseOverScrollViewer is not null && !ReferenceEquals(mouseOverScrollViewer, _scrollViewer))
			return;

		_scrollViewer.ScrollToHorizontalOffset(_scrollViewer.HorizontalOffset - e.Delta);
		e.Handled = true;

		return;


		//// Local Functions


		ScrollViewer? GetLowestHorizontallyScrollableScrollViewerMouseIsOver()
		{
			// Get the lowest enabled element the mouse is over
			var dp = e.MouseDevice.DirectlyOver as DependencyObject;

			// Walk the tree upwards and find the first ScrollViewer that can scroll horizontally
			while
			(
				dp is not null &&
				(
					dp is not ScrollViewer ||
					(dp is ScrollViewer sv && !CanScrollHorizontally(sv))
				)
			)
				dp = VisualTreeHelper.GetParent(dp);

			return dp as ScrollViewer;
		}

		bool CanScrollHorizontally(ScrollViewer scrollViewer) =>
 			scrollViewer.ScrollableWidth > 0 &&	
			scrollViewer.HorizontalScrollBarVisibility != ScrollBarVisibility.Disabled;

	}



	//// Event Handlers


	void OnScrollViewerPreviewMouseWheel(object sender, MouseWheelEventArgs e) =>
		ProcessShiftMouseWheel(sender, e);

	void OnLoaded(object sender, RoutedEventArgs e) =>
	RegisterMouseWheelHook();

	void OnUnloaded(object sender, RoutedEventArgs e) =>
		UnregisterMouseWheelHook();



	//// Native


	const int WM_MOUSEHWHEEL = 0x020E;

	void RegisterMouseWheelHook()
	{
		if (_scrollViewer is null)
			return;

		var source = PresentationSource.FromVisual(_scrollViewer) as HwndSource;
		source?.AddHook(WndProc);
	}

	void UnregisterMouseWheelHook()
	{
		if (_scrollViewer is null)
			return;

		var source = PresentationSource.FromVisual(_scrollViewer) as HwndSource;
		source?.AddHook(WndProc);
	}

	/// <summary>Hook process </summary>
	IntPtr WndProc(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled)
	{
		if (_scrollViewer is null)
			return IntPtr.Zero;

		// Only scroll if pointer is over this control
		if (!_scrollViewer.IsMouseOver)
			return IntPtr.Zero;

		if (msg != WM_MOUSEHWHEEL)
			return IntPtr.Zero;

		// Extract delta and scroll
		int delta = (short)((wParam.ToInt64() >> 16) & 0xFFFF);
		_scrollViewer.ScrollToHorizontalOffset(_scrollViewer.HorizontalOffset + delta);
		handled = true;

		return IntPtr.Zero;
	}


}
