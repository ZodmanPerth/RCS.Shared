using RCS.Services.Native.Windows;
using RCS.Windows;
using System.Windows.Media;
using System.Windows.Shell;

namespace System.Windows;

public static class SystemWindowsExtensions
{
	static IWindowService? _windowService = null;


	public static void Initialise(IWindowService windowService) =>
		_windowService = windowService ?? throw new ArgumentNullException(nameof(windowService));



	//// Fluent configuration


	/// <summary>Returns the window with the title applied</summary>
	public static WindowBase WithTitle(this WindowBase window, string title)
	{
		window.Title = title;
		return window;
	}

	/// <summary>Returns the window with the startup location applied</summary>
	public static WindowBase WithStartupLocation(this WindowBase window, WindowStartupLocation startupLocation)
	{
		window.WindowStartupLocation = startupLocation;
		return window;
	}

	/// <summary>Returns the window with the show in taskbar state applied</summary>
	public static WindowBase WithShowInTaskbar(this WindowBase window, bool isShownInTaskbar)
	{
		window.ShowInTaskbar = isShownInTaskbar;
		return window;
	}

	/// <summary>Returns the Window with the set width and height</summary>
	public static WindowBase WithSize(this WindowBase window, double width, double height)
	{
		window.Width = width;
		window.Height = height;
		return window;
	}

	/// <summary>Returns the Window at a new position</summary>
	public static WindowBase WithPosition(this WindowBase window, double Top, double Left)
	{
		window.Top = Top;
		window.Left = Left;
		return window;
	}

	/// <summary>Returns the window with no chrome</summary>
	public static WindowBase WithNoChrome(this WindowBase window)
	{
		window.WindowStyle = WindowStyle.None;
		return window;
	}

	/// <summary>Returns the window with a single border</summary>
	public static WindowBase WithSingleBorder(this WindowBase window)
	{
		window.WindowStyle = WindowStyle.SingleBorderWindow;
		return window;
	}

	/// <summary>Returns the window with no transparency</summary>
	public static WindowBase WithTransparentBackground(this WindowBase window)
	{
		window.Background = Brushes.Transparent;
		return window;
	}

	/// <summary>Returns the window with no title bar</summary>
	/// <remarks>This also removes the draggable area</remarks>
	public static WindowBase WithNoTitleBar(this WindowBase window)
	{
		var chrome = WindowChrome.GetWindowChrome(window) ?? new WindowChrome();
		chrome.CaptionHeight = 0;
		WindowChrome.SetWindowChrome(window, chrome);
		return window;
	}

	/// <summary>Returns the window with the title bar height set</summary>
	public static WindowBase WithTitleBarHeight(this WindowBase window, int height)
	{
		var chrome = WindowChrome.GetWindowChrome(window) ?? new WindowChrome();
		chrome.CaptionHeight = height;
		WindowChrome.SetWindowChrome(window, chrome);
		return window;
	}

	/// <summary>Returns the window with the frame thickness set</summary>
	public static WindowBase WithFrameThickness(this WindowBase window, Thickness frame)
	{
		var chrome = WindowChrome.GetWindowChrome(window) ?? new WindowChrome();
		chrome.GlassFrameThickness = frame;
		WindowChrome.SetWindowChrome(window, chrome);
		return window;
	}

	/// <summary>Returns the window with the resize border thickness set</summary>
	public static WindowBase WithResizeBorderThickness(this WindowBase window, Thickness borderThickness)
	{
		var chrome = WindowChrome.GetWindowChrome(window) ?? new WindowChrome();
		chrome.ResizeBorderThickness = borderThickness;
		WindowChrome.SetWindowChrome(window, chrome);
		return window;
	}


	/// <summary>Configures the window for using <see cref="WindowChromeEx"/></summary>
	public static WindowBase ConfigureForWindowChromeControl(this WindowBase window)
	{
		if (_windowService is null)
			throw new NullReferenceException($"{nameof(_windowService)} must be initialised before calling this function");

		window
			.WithTransparentBackground()
			.WithTitleBarHeight(0)
			.WithResizeBorderThickness(new(5));

		window.Loaded += (s, e) =>
		{
			var colour = Color.FromRgb(242, 242, 249);  // Windows 11 title bar colour
			_windowService.SetTitleBarColour(window, colour);
		};

		return window;
	}



	//// Basics


	/// <summary>The rect translated by an amount</summary>
	public static Rect TranslateBy(this Rect rect, double offsetX, double offsetY) =>
		new Rect(rect.X + offsetX, rect.Y + offsetY, rect.Width, rect.Height);

	/// <summary>The centre point of a rect</summary>
	public static Point Centre(this Rect rect) =>
		new Point((rect.Right + rect.Left) / 2.0, (rect.Bottom + rect.Top) / 2.0);

	/// <summary>Returns a new Rect that is the intersection of both Rects</summary>
	/// <remarks>
	/// System.Windows.Intersect methods have side effect and alter the original Rect.
	/// This extension is safe because it returns a new Rect containing the result
	/// without affecting the existing Rects
	/// </remarks>
	public static Rect IntersectSafe(this Rect rect, Rect rect2)
	{
		var newRect = rect;
		newRect.Intersect(rect2);
		return newRect;
	}

	/// <summary>The area of the rect</summary>
	public static double Area(this Rect rect) =>
		rect.Width * rect.Height;

	/// <summary>The distance from the point to a second point</summary>
	public static double DistanceTo(this Point from, Point to)
	{
		var x1 = from.X;
		var y1 = from.Y;
		var x2 = to.X;
		var y2 = to.Y;

		var distance = Math.Sqrt(Math.Pow(x1 - x2, 2) + Math.Pow(y1 - y2, 2));
		return distance;
	}



	//// Complex


	/// <summary>Returns the container the Rect is closest to</summary>
	/// <remarks>
	/// ALGORITHM:<br></br>
	/// Returns the container that completely contains the rect.<br></br>
	/// If there are none, returns the container with the biggest overlapping area.<br></br>
	/// If there are more than one, returns the first container that contains either the TopLeft or BottomLeft of the rect.<br></br>
	/// If there are none, retuns the container with closest centre to rect.<br></br>
	/// If there are more than one, returns the first container with the closest centre.<br></br>
	/// <br></br>
	/// NOTE: <br></br>
	/// The "closest centre" comparison isn't correct all cases.  To correctly resolve required more complicated computation
	/// that isn't justified at the moment.
	/// </remarks>
	public static Rect ClosestContainer(this Rect rect, IEnumerable<Rect> containers, Rect? defaultContainer = null)
	{
		var defaultResult = defaultContainer ?? new Rect();

		if (containers is null || !containers.Any())
			return defaultResult;

		var containerArray = containers.ToArray();

		// Check if completely contained within an existing container
		var containingContainers = containerArray
			.Where(_ => _.Contains(rect))
			.ToArray();
		if (containingContainers.Length == 1)
			return containingContainers[0];

		// Check if the rect only intersects with one container
		var intersectedContainers = containerArray
			.Where(_ => Rect.Intersect(rect, _) != Rect.Empty)
			.ToArray();
		if (intersectedContainers.Length == 1)
			return intersectedContainers[0];

		// Check the container with the biggest overlapping area
		if (intersectedContainers.Any())
		{
			var biggestOverlapContainers = intersectedContainers
				.Select(_ => new { NewRect = rect, Container = _ })
				.Select(_ => new { IntersectedRect = _.NewRect.IntersectSafe(_.Container), _.Container })
				.Select(_ => new { _.IntersectedRect, Area = _.IntersectedRect.Area(), _.Container })
				.ToArray();
			var maxOverlap = biggestOverlapContainers.Max(_ => _.Area);
			var containersWithBiggestOverlap = biggestOverlapContainers
				.Where(_ => _.Area == maxOverlap)
				.ToArray();
			if (containersWithBiggestOverlap.Length == 1)
				return containersWithBiggestOverlap[0].Container;
			else if (containersWithBiggestOverlap.Length > 0)
			{
				foreach (var containerDetails in containersWithBiggestOverlap)
				{
					if (containerDetails.Container.Contains(rect.TopLeft))
						return containerDetails.Container;
					else if (containerDetails.Container.Contains(rect.BottomLeft))
						return containerDetails.Container;
				}
			}
		}

		// Return the container with the closest centre
		var rectCentre = rect.Centre();
		var centreDistanceContainers = containerArray
			.Select(_ => new { Centre = _.Centre(), Container = _ })
			.Select(_ => new { DistanceBetweenCentres = _.Centre.DistanceTo(rectCentre), _.Container })
			.ToArray();
		var minDistance = centreDistanceContainers.Min(_ => _.DistanceBetweenCentres);
		var containersWithSmallestDistanceBetweenCentres = centreDistanceContainers
			.Where(_ => _.DistanceBetweenCentres == minDistance)
			.ToArray();

		// There will be one or more results.  Just return the first one.
		return containersWithSmallestDistanceBetweenCentres[0].Container;
	}

	/// <summary>Returns a rect that is moved and resized to fit inside the <paramref name="fitTo"/> rect.</summary>
	public static Rect FitTo(this Rect rect, Rect fitTo)
	{
		var result = rect;   // copy value

		// Check if it already fits
		if (fitTo.Contains(rect))
			return result;

		var isRectWider = rect.Width > fitTo.Width;
		var isRectTaller = rect.Height > fitTo.Height;

		// Check if it's bigger
		var isRectBigger = isRectWider && isRectTaller;
		if (isRectBigger)
		{
			result = fitTo;     // copy value
			return result;
		}

		// At this point:
		// * Rect is not contained
		//   * Could be intersecting our outside
		// * Rect is smaller, at least in 1 dimension

		//var translateX = isRectWider ? rect.Width - fitTo.Width : fitTo.Width - rect.Width;
		//var translateY = isRectTaller ? rect.Height - fitTo.Height :  fitTo.Height - rect.Height;
		var isLeftEdgeOutside = rect.Left < fitTo.Left;
		var isRightEdgeOutside = rect.Right > fitTo.Right;
		var isTopEdgeOutside = rect.Top < fitTo.Top;
		var isBottomEdgeOutside = rect.Bottom > fitTo.Bottom;

		var rightEdgeOverhang = isRightEdgeOutside
			? rect.Right - fitTo.Right
			: 0;

		var bottomEdgeOverhang = isBottomEdgeOutside
			? rect.Bottom - fitTo.Bottom
			: 0;

		var newX = isRectWider
			? fitTo.Left
			: isLeftEdgeOutside
				? fitTo.Left
				: isRightEdgeOutside
					? rect.Left - rightEdgeOverhang
					: rect.Left;

		var newY = isRectTaller
			? fitTo.Top
			: isTopEdgeOutside
				? fitTo.Top
				: isBottomEdgeOutside
					? rect.Top - bottomEdgeOverhang
					: rect.Top;

		var newWidth = isRectWider
			? fitTo.Width
			: rect.Width;

		var newHeight = isRectTaller
			? fitTo.Height
			: rect.Height;

		return new Rect(newX, newY, newWidth, newHeight);
	}
}
