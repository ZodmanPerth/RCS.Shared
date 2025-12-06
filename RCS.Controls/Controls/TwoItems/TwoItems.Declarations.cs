using System.Windows;
using System.Windows.Controls;

#nullable disable

namespace RCS.Controls;

public partial class TwoItems : Control
{
	/// <summary>The GridLength of the left column</summary>
	public static readonly DependencyProperty LeftColumnGridLengthProperty = DependencyProperty.RegisterAttached
	(
		"LeftColumnGridLength",
		typeof(GridLength),
		typeof(TwoItems),
		new FrameworkPropertyMetadata(defaultValue: new GridLength(1d, GridUnitType.Auto), flags: FrameworkPropertyMetadataOptions.AffectsRender)
	);
	public GridLength LeftColumnGridLength
	{
		get => (GridLength)GetValue(LeftColumnGridLengthProperty);
		set => SetValue(LeftColumnGridLengthProperty, value);
	}

	/// <summary>The GridLength of the gutter column</summary>
	public static readonly DependencyProperty GutterColumnGridLengthProperty = DependencyProperty.RegisterAttached
	(
		"GutterColumnGridLength",
		typeof(GridLength),
		typeof(TwoItems),
		new FrameworkPropertyMetadata(defaultValue: new GridLength(1d, GridUnitType.Auto), flags: FrameworkPropertyMetadataOptions.AffectsRender)
	);
	public GridLength GutterColumnGridLength
	{
		get => (GridLength)GetValue(GutterColumnGridLengthProperty);
		set => SetValue(GutterColumnGridLengthProperty, value);
	}

	/// <summary>The GridLength of the right column</summary>
	public static readonly DependencyProperty RightColumnGridLengthProperty = DependencyProperty.RegisterAttached
	(
		"RightColumnGridLength",
		typeof(GridLength),
		typeof(TwoItems),
		new FrameworkPropertyMetadata(defaultValue: new GridLength(1d, GridUnitType.Auto), flags: FrameworkPropertyMetadataOptions.AffectsRender)
	);
	public GridLength RightColumnGridLength
	{
		get => (GridLength)GetValue(RightColumnGridLengthProperty);
		set => SetValue(RightColumnGridLengthProperty, value);
	}



	/// <summary>The margin of the left item</summary>
	public static readonly DependencyProperty LeftItemMarginProperty = DependencyProperty.RegisterAttached
	(
		"LeftItemMargin",
		typeof(Thickness),
		typeof(TwoItems),
		new FrameworkPropertyMetadata(defaultValue: new Thickness(), flags: FrameworkPropertyMetadataOptions.AffectsRender)
	);
	public Thickness LeftItemMargin
	{
		get => (Thickness)GetValue(LeftItemMarginProperty);
		set => SetValue(LeftItemMarginProperty, value);
	}

	/// <summary>The margin of the right item</summary>
	public static readonly DependencyProperty RightItemMarginProperty = DependencyProperty.RegisterAttached
	(
		"RightItemMargin",
		typeof(Thickness),
		typeof(TwoItems),
		new FrameworkPropertyMetadata(defaultValue: new Thickness(), flags: FrameworkPropertyMetadataOptions.AffectsRender)
	);
	public Thickness RightItemMargin
	{
		get => (Thickness)GetValue(RightItemMarginProperty);
		set => SetValue(RightItemMarginProperty, value);
	}



	/// <summary>The content of the left item</summary>
	public static readonly DependencyProperty LeftItemContentProperty = DependencyProperty.RegisterAttached
	(
		"LeftItemContent",
		typeof(object),
		typeof(TwoItems),
		new FrameworkPropertyMetadata(defaultValue: default, flags: FrameworkPropertyMetadataOptions.AffectsRender)
	);
	public object LeftItemContent
	{
		get => (object)GetValue(LeftItemContentProperty);
		set => SetValue(LeftItemContentProperty, value);
	}

	/// <summary>The content template of the left item</summary>
	public static readonly DependencyProperty LeftItemContentTemplateProperty = DependencyProperty.RegisterAttached
	(
		"LeftItemContentTemplate",
		typeof(DataTemplate),
		typeof(TwoItems),
		new FrameworkPropertyMetadata(defaultValue: default, flags: FrameworkPropertyMetadataOptions.AffectsRender)
	);
	public DataTemplate LeftItemContentTemplate
	{
		get => (DataTemplate)GetValue(LeftItemContentTemplateProperty);
		set => SetValue(LeftItemContentTemplateProperty, value);
	}



	/// <summary>The content of the right item</summary>
	public static readonly DependencyProperty RightItemContentProperty = DependencyProperty.RegisterAttached
	(
		"RightItemContent",
		typeof(object),
		typeof(TwoItems),
		new FrameworkPropertyMetadata(defaultValue: default, flags: FrameworkPropertyMetadataOptions.AffectsRender)
	);
	public object RightItemContent
	{
		get => (object)GetValue(RightItemContentProperty);
		set => SetValue(RightItemContentProperty, value);
	}

	/// <summary>The content template of the right item</summary>
	public static readonly DependencyProperty RightItemContentTemplateProperty = DependencyProperty.RegisterAttached
	(
		"RightItemContentTemplate",
		typeof(DataTemplate),
		typeof(TwoItems),
		new FrameworkPropertyMetadata(defaultValue: default, flags: FrameworkPropertyMetadataOptions.AffectsRender)
	);
	public DataTemplate RightItemContentTemplate
	{
		get => (DataTemplate)GetValue(RightItemContentTemplateProperty);
		set => SetValue(RightItemContentTemplateProperty, value);
	}
}