using System.Windows;
using System.Windows.Controls;

namespace OKB.Controls;

public partial class ToggleSwitch : CheckBox
{
	/// <summary>true when the switch is on, false when off</summary>
	public new bool IsChecked
	{
		get => (bool)GetValue(IsCheckedProperty);
		set => SetValue(IsCheckedProperty, value);
	}
	public static new readonly DependencyProperty IsCheckedProperty = DependencyProperty.Register
	(
		nameof(IsChecked),
		typeof(bool),
		typeof(ToggleSwitch),
		new PropertyMetadata(false, (d, e) => (d as ToggleSwitch)?.OnCheckedPropertyChanged(e))
	);
	void OnCheckedPropertyChanged(DependencyPropertyChangedEventArgs e)
	{
		// Ensure the base control is in sync, especially when first loaded and bound
		base.IsChecked = IsChecked;
	}



	/// <summary>The border thickness of the housing</summary>
	public Thickness HousingBorderThickness
	{
		get => (Thickness)GetValue(HousingBorderThicknessProperty);
		set => SetValue(HousingBorderThicknessProperty, value);
	}
	public static readonly DependencyProperty HousingBorderThicknessProperty = DependencyProperty.Register
	(
		nameof(HousingBorderThickness),
		typeof(Thickness),
		typeof(ToggleSwitch),
		new PropertyMetadata(new Thickness(2), (d, e) => (d as ToggleSwitch)?.OnHousingBorderThicknessPropertyChanged(e))
	);
	void OnHousingBorderThicknessPropertyChanged(DependencyPropertyChangedEventArgs e)
	{
		SynchroniseHousingInnerWidth();
		SynchroniseHousingInnerHeight();
	}

	/// <summary>THe width of the housing</summary>
	public double HousingWidth
	{
		get => (double)GetValue(HousingWidthProperty);
		set => SetValue(HousingWidthProperty, value);
	}
	public static readonly DependencyProperty HousingWidthProperty = DependencyProperty.Register
	(
		nameof(HousingWidth),
		typeof(double),
		typeof(ToggleSwitch),
		new PropertyMetadata(42d, (d, e) => (d as ToggleSwitch)?.OnHousingWidthPropertyChanged(e))
	);
	void OnHousingWidthPropertyChanged(DependencyPropertyChangedEventArgs e)
	{
		SynchroniseHousingInnerWidth();
		SynchroniseLeverRightPosition();
		SynchroniseHousingInnerCornerRadius();
	}

	/// <summary>Internal calculated width of the inner housing (border)</summary>
	internal double HousingInnerWidth
	{
		get => (double)GetValue(HousingInnerWidthProperty);
		set => SetValue(HousingInnerWidthProperty, value);
	}
	internal static readonly DependencyProperty HousingInnerWidthProperty = DependencyProperty.Register
	(
		nameof(HousingInnerWidth),
		typeof(double),
		typeof(ToggleSwitch),
		new PropertyMetadata(40d)
	);

	/// <summary>The height of the housing</summary>
	public double HousingHeight
	{
		get => (double)GetValue(HousingHeightProperty);
		set => SetValue(HousingHeightProperty, value);
	}
	public static readonly DependencyProperty HousingHeightProperty = DependencyProperty.Register
	(
		nameof(HousingHeight),
		typeof(double),
		typeof(ToggleSwitch),
		new PropertyMetadata(20d, (d, e) => (d as ToggleSwitch)?.OnHousingHeightPropertyChanged(e))
	);
	void OnHousingHeightPropertyChanged(DependencyPropertyChangedEventArgs e)
	{
		SynchroniseHousingInnerHeight();
	}

	/// <summary>Internal calculated height of the inner housing (border)</summary>
	internal double HousingInnerHeight
	{
		get => (double)GetValue(HousingInnerHeightProperty);
		set => SetValue(HousingInnerHeightProperty, value);
	}
	internal static readonly DependencyProperty HousingInnerHeightProperty = DependencyProperty.Register
	(
		nameof(HousingInnerHeight),
		typeof(double),
		typeof(ToggleSwitch),
		new PropertyMetadata(18d)
	);


	/// <summary>The corner radius of the housing</summary>
	public CornerRadius HousingCornerRadius
	{
		get => (CornerRadius)GetValue(HousingCornerRadiusProperty);
		set => SetValue(HousingCornerRadiusProperty, value);
	}
	public static readonly DependencyProperty HousingCornerRadiusProperty = DependencyProperty.Register
	(
		nameof(HousingCornerRadius),
		typeof(CornerRadius),
		typeof(ToggleSwitch),
		new PropertyMetadata(new CornerRadius(10), (d, e) => (d as ToggleSwitch)?.OnHousingCornerRadiusPropertyChanged(e))
	);
	void OnHousingCornerRadiusPropertyChanged(DependencyPropertyChangedEventArgs e)
	{
		SynchroniseHousingInnerCornerRadius();
	}

	/// <summary>Internal calculated corner radius of the housing</summary>
	internal CornerRadius HousingInnerCornerRadius
	{
		get => (CornerRadius)GetValue(HousingInnerCornerRadiusProperty);
		set => SetValue(HousingInnerCornerRadiusProperty, value);
	}
	internal static readonly DependencyProperty HousingInnerCornerRadiusProperty = DependencyProperty.Register
	(
		nameof(HousingInnerCornerRadius),
		typeof(CornerRadius),
		typeof(ToggleSwitch),
		new PropertyMetadata(new CornerRadius(8))
	);


	/// <summary>The width of the lever (inside the housing)</summary>
	public double LeverWidth
	{
		get => (double)GetValue(LeverWidthProperty);
		set => SetValue(LeverWidthProperty, value);
	}
	public static readonly DependencyProperty LeverWidthProperty = DependencyProperty.Register
	(
		nameof(LeverWidth),
		typeof(double),
		typeof(ToggleSwitch),
		new PropertyMetadata(11d, (d, e) => (d as ToggleSwitch)?.OnLeverWidthPropertyChanged(e))
	);
	void OnLeverWidthPropertyChanged(DependencyPropertyChangedEventArgs e)
	{
		SynchroniseLeverRightPosition();
	}


	/// <summary>The horizontal space between the lever and the housing</summary>
	public double LeverHorizontalSpace
	{
		get => (double)GetValue(LeverHorizontalSpaceProperty);
		set => SetValue(LeverHorizontalSpaceProperty, value);
	}
	public static readonly DependencyProperty LeverHorizontalSpaceProperty = DependencyProperty.Register
	(
		nameof(LeverHorizontalSpace),
		typeof(double),
		typeof(ToggleSwitch),
		new PropertyMetadata(5d, (d, e) => (d as ToggleSwitch)?.OnLeverHorizontalSpacePropertyChanged(e))
	);
	void OnLeverHorizontalSpacePropertyChanged(DependencyPropertyChangedEventArgs e)
	{
		LeverMargin = new Thickness(LeverHorizontalSpace, 0, LeverHorizontalSpace, 0);
		SynchroniseLeverRightPosition();
	}

	/// <summary>Internal calculated margin of the lever</summary>
	internal Thickness LeverMargin
	{
		get => (Thickness)GetValue(LeverMarginProperty);
		set => SetValue(LeverMarginProperty, value);
	}
	internal static readonly DependencyProperty LeverMarginProperty = DependencyProperty.Register
	(
		nameof(LeverMargin),
		typeof(Thickness),
		typeof(ToggleSwitch),
		new PropertyMetadata(new Thickness(5, 0, 5, 0))
	);




	/// <summary>The duration of the animation</summary>
	public Duration AnimationDuration
	{
		get => (Duration)GetValue(AnimationDurationProperty);
		set => SetValue(AnimationDurationProperty, value);
	}
	public static readonly DependencyProperty AnimationDurationProperty = DependencyProperty.Register
	(
		nameof(AnimationDuration),
		typeof(Duration),
		typeof(ToggleSwitch),
		new PropertyMetadata(new Duration(TimeSpan.FromSeconds(0.05)), (d, e) => (d as ToggleSwitch)?.OnDurationPropertyChanged(e))
	);
	void OnDurationPropertyChanged(DependencyPropertyChangedEventArgs e)
	{
		SynchroniseAnimationDuration();
	}

	/// <summary>Animation plays when changing state when true</summary>
	public bool IsAnimationAllowed
	{
		get => (bool)GetValue(IsAnimationAllowedProperty);
		set => SetValue(IsAnimationAllowedProperty, value);
	}
	public static readonly DependencyProperty IsAnimationAllowedProperty = DependencyProperty.Register
	(
		nameof(IsAnimationAllowed),
		typeof(bool),
		typeof(ToggleSwitch),
		new PropertyMetadata(true)
	);
}