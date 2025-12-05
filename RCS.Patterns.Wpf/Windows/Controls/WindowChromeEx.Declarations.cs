using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

#nullable disable

namespace OKB.Windows.Controls;

public partial class WindowChromeEx : Control
{
	/// <summary>The content of the window</summary>
	public static readonly DependencyProperty ContentProperty = DependencyProperty.RegisterAttached
	(
		"Content",
		typeof(object),
		typeof(WindowChromeEx),
		new FrameworkPropertyMetadata(defaultValue: default, flags: FrameworkPropertyMetadataOptions.AffectsRender)
	);
	public object Content
	{
		get => (object)GetValue(ContentProperty);
		set => SetValue(ContentProperty, value);
	}

	/// <summary>The content template of the window</summary>
	public static readonly DependencyProperty ContentTemplateProperty = DependencyProperty.RegisterAttached
	(
		"ContentTemplate",
		typeof(DataTemplate),
		typeof(WindowChromeEx),
		new FrameworkPropertyMetadata(defaultValue: default, flags: FrameworkPropertyMetadataOptions.AffectsRender)
	);
	public DataTemplate ContentTemplate
	{
		get => (DataTemplate)GetValue(ContentTemplateProperty);
		set => SetValue(ContentTemplateProperty, value);
	}

	/// <summary>The content of the title bar</summary>
	public static readonly DependencyProperty TitleBarContentProperty = DependencyProperty.RegisterAttached
	(
		"TitleBarContent",
		typeof(object),
		typeof(WindowChromeEx),
		new FrameworkPropertyMetadata(defaultValue: default, flags: FrameworkPropertyMetadataOptions.AffectsRender)
	);
	public object TitleBarContent
	{
		get => (object)GetValue(TitleBarContentProperty);
		set => SetValue(TitleBarContentProperty, value);
	}

	/// <summary>The content template of the title bar</summary>
	public static readonly DependencyProperty TitleBarContentTemplateProperty = DependencyProperty.RegisterAttached
	(
		"TitleBarContentTemplate",
		typeof(DataTemplate),
		typeof(WindowChromeEx),
		new FrameworkPropertyMetadata(defaultValue: default, flags: FrameworkPropertyMetadataOptions.AffectsRender)
	);
	public DataTemplate TitleBarContentTemplate
	{
		get => (DataTemplate)GetValue(TitleBarContentTemplateProperty);
		set => SetValue(TitleBarContentTemplateProperty, value);
	}


	/// <summary>The colour of the chrome</summary>
	public static readonly DependencyProperty ChromeBrushProperty = DependencyProperty.RegisterAttached
	(
		"ChromeBrush",
		typeof(Brush),
		typeof(WindowChromeEx),
		new FrameworkPropertyMetadata(defaultValue: Brushes.Magenta, flags: FrameworkPropertyMetadataOptions.AffectsRender)
	);
	public Brush ChromeBrush
	{
		get => (Brush)GetValue(ChromeBrushProperty);
		set => SetValue(ChromeBrushProperty, value);
	}
}