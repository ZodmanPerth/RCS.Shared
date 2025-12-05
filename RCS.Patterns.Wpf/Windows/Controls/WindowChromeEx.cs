using OKB.Services.Native.Windows;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;

#nullable disable

namespace OKB.Windows.Controls;

[TemplatePart(Name = PartWindowChromeBorder, Type = typeof(Border))]
[TemplatePart(Name = PartTitleBarRow, Type = typeof(RowDefinition))]
[TemplatePart(Name = PartIconMenu, Type = typeof(Rectangle))]
[TemplatePart(Name = PartWindowDragBar, Type = typeof(Rectangle))]
[TemplatePart(Name = PartTitleBarFiller, Type = typeof(Rectangle))]
[TemplatePart(Name = PartTitleBarFiller, Type = typeof(Rectangle))]
[TemplatePart(Name = PartColumnnDefinitionTitleBarContent, Type = typeof(ColumnDefinition))]
[TemplatePart(Name = PartColumnnDefinitionNoGoZone, Type = typeof(ColumnDefinition))]
public partial class WindowChromeEx : Control
{
	const string PartWindowChromeBorder = "PART_WindowChromeBorder";
	const string PartTitleBarRow = "PART_TitleBarRow";
	const string PartIconMenu = "PART_IconMenu";
	const string PartWindowDragBar = "PART_WindowDragBar";
	const string PartTitleBarFiller = "PART_TitleBarFiller";
	const string PartColumnnDefinitionTitleBarContent = "PART_ColumnnDefinitionTitleBarContent";
	const string PartColumnnDefinitionNoGoZone = "PART_ColumnnDefinitionNoGoZone";

	Border _windowChromeBorder = null;
	RowDefinition _titleBarRow = null;
	Rectangle _iconMenu = null;
	Rectangle _windowDragBar = null;
	Rectangle _titleBarFiller = null;
	ColumnDefinition _columnDefinitionTitleBarContent;
	ColumnDefinition _columnDefinitionNoGoZone;

	Window _window;
	IWindowService _windowService;

	bool _isInitialised = false;



	/// <summary>The size of the icon section</summary>
	public Size IconSectionSize => new(_iconMenu?.ActualWidth ?? 0, _iconMenu?.ActualHeight ?? 0);

	/// <summary>The size of the usable area</summary>
	public Size UsableSectionSize => new(_columnDefinitionTitleBarContent?.ActualWidth ?? 0, _titleBarRow?.ActualHeight ?? 0);

	/// <summary>The size of the no-go zone</summary>
	public Size NoGoZoneSectionSize => new(_columnDefinitionNoGoZone?.ActualWidth ?? 0, _titleBarRow?.ActualHeight ?? 0);



	//// Lifecycle


	public void Initialise(IWindowService windowService)
	{
		_windowService = windowService ?? throw new ArgumentNullException(nameof(windowService));
		_isInitialised = true;
		SynchroniseInitialisationVisualisation();
	}



	//// Overrides


	public override void OnApplyTemplate()
	{
		base.OnApplyTemplate();

		_windowChromeBorder = GetTemplateChild(PartWindowChromeBorder) as Border;
		_titleBarRow = GetTemplateChild(PartTitleBarRow) as RowDefinition;

		_iconMenu = GetTemplateChild(PartIconMenu) as Rectangle;
		_iconMenu.MouseLeftButtonDown += ShowSystemMenu;
		_iconMenu.MouseRightButtonDown += ShowSystemMenu;

		_windowDragBar = GetTemplateChild(PartWindowDragBar) as Rectangle;
		_windowDragBar.MouseLeftButtonDown += OnWindowDragBarMouseLeftButtonDown;
		_windowDragBar.MouseRightButtonDown += ShowSystemMenu;

		_titleBarFiller = GetTemplateChild(PartTitleBarFiller) as Rectangle;

		_columnDefinitionTitleBarContent = GetTemplateChild(PartColumnnDefinitionTitleBarContent) as ColumnDefinition;
		_columnDefinitionNoGoZone = GetTemplateChild(PartColumnnDefinitionNoGoZone) as ColumnDefinition;

		SynchroniseInitialisationVisualisation();
		InitialiseWindow();


		return;


		//// Local Functions


		void InitialiseWindow()
		{
			_window = Window.GetWindow(this);
			if (_window is null)
				return;

			_window.StateChanged += OnWindowStateChanged;
		}
	}



	//// Helpers


	void SynchroniseInitialisationVisualisation()
	{
		if (!_isInitialised)
			VisualiseInitialisedState(new SolidColorBrush(Colors.Magenta), "WindowChromeEx is not initialised");
		else
			VisualiseInitialisedState(ChromeBrush);

		return;


		//// Local Functions


		void VisualiseInitialisedState(Brush brush, string toolTip = null)
		{
			_windowChromeBorder.BorderBrush = brush;
			_windowDragBar.Fill = brush;
			_iconMenu.Fill = brush;
			_titleBarFiller.Fill = brush;

			_windowChromeBorder.ToolTip = toolTip;
			_windowDragBar.ToolTip = toolTip;
			_iconMenu.ToolTip = toolTip;
			_titleBarFiller.ToolTip = toolTip;
		}
	}



	//// Event Handlers


	void OnWindowStateChanged(object sender, EventArgs e)
	{
		Debug.Assert(_window is not null);

		var isWindowMaximised = _window.WindowState == WindowState.Maximized;

		(_windowChromeBorder.BorderThickness, _titleBarRow.Height) = isWindowMaximised
			? (new Thickness(8, 4, 8, 8), new GridLength(27, GridUnitType.Pixel))
			: (new Thickness(4, 0, 4, 4), new GridLength(31, GridUnitType.Pixel));
	}

	void ShowSystemMenu(object sender, MouseButtonEventArgs e)
	{
		if (_window is null || _windowService is null)
			return;

		var point = e.GetPosition(this).TranslatedBy(_window.Left, _window.Top);
		_windowService.ShowSystemMenu(_window, point);
	}

	void OnWindowDragBarMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
	{
		if (_window is null)
			return;

		if (e.ClickCount == 2)
			_window.WindowState = _window.WindowState == WindowState.Normal
				? WindowState.Maximized
				: WindowState.Normal;
		else
			_window.DragMove();
	}
}