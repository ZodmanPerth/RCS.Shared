using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Animation;

namespace RCS.Controls;

[TemplatePart(Name = PartLever, Type = typeof(FrameworkElement))]
public partial class ToggleSwitch : CheckBox
{
	const string PartLever = "PART_Lever";

	FrameworkElement? lever = null;

	Storyboard? _toggleOnStoryboard = null;
	Storyboard? _toggleOffStoryboard = null;
	DoubleAnimation? _toggleOnAnimation = null;
	DoubleAnimation? _toggleOffAnimation = null;

	double _leverRightPosition = 21d;



	//// Helpers


	void SynchroniseHousingInnerWidth() =>
		HousingInnerWidth = HousingWidth - HousingBorderThickness.Left - HousingBorderThickness.Right;

	void SynchroniseHousingInnerHeight() =>
		HousingInnerHeight = HousingHeight - HousingBorderThickness.Top - HousingBorderThickness.Bottom;

	void SynchroniseHousingInnerCornerRadius()
	{
		var topLeft = HousingCornerRadius.TopLeft - Math.Max(HousingBorderThickness.Top, HousingBorderThickness.Left);
		var topRight = HousingCornerRadius.TopRight - Math.Max(HousingBorderThickness.Top, HousingBorderThickness.Right);
		var bottomRight = HousingCornerRadius.BottomRight - Math.Max(HousingBorderThickness.Bottom, HousingBorderThickness.Right);
		var bottomLeft = HousingCornerRadius.BottomLeft - Math.Max(HousingBorderThickness.Bottom, HousingBorderThickness.Left);
		HousingInnerCornerRadius = new CornerRadius(topLeft, topRight, bottomRight, bottomLeft);
	}

	void SynchroniseLeverRightPosition()
	{
		_leverRightPosition = HousingWidth - LeverWidth - 2 * LeverHorizontalSpace;

		if (_toggleOnAnimation is not null)
			_toggleOnAnimation.To = _leverRightPosition;
		
		if (_toggleOffAnimation is not null)
			_toggleOffAnimation.From = _leverRightPosition;

		// Ensure lever is in correct position when checked
		if (IsChecked == true)
			PlayStoryboard(_toggleOnStoryboard, isAnimationAllowed: false);
	}

	void SynchroniseAnimationDuration()
	{
		if (_toggleOnAnimation is not null)
			_toggleOnAnimation.Duration = AnimationDuration;

		if (_toggleOffAnimation is not null)
			_toggleOffAnimation.Duration = AnimationDuration;
	}

	void PlayStoryboard(Storyboard? storyboard, bool isAnimationAllowed = true)
	{
		if (storyboard is null)
			return;

		storyboard.Begin(this, isControllable: true);

		if (!IsAnimationAllowed || !isAnimationAllowed)
			storyboard.SkipToFill(this);
	}

	void DoIsCheckedChanged()
	{
		if (IsChecked)
			PlayStoryboard(_toggleOnStoryboard);
		else
			PlayStoryboard(_toggleOffStoryboard);
	}


	//// Overrides


	public override void OnApplyTemplate()
	{
		base.OnApplyTemplate();

		lever = GetTemplateChild(PartLever) as FrameworkElement;
		
		CreateAnimations();

		SynchroniseLeverRightPosition();
		SynchroniseAnimationDuration();

		return;


		//// Local Functions


		void CreateAnimations()
		{
			_toggleOnAnimation = new() { From = 0 };
			_toggleOnStoryboard = CreateStoryboard(_toggleOnAnimation);

			_toggleOffAnimation = new() { To = 0 };
			_toggleOffStoryboard = CreateStoryboard(_toggleOffAnimation);
		}

		Storyboard CreateStoryboard(DoubleAnimation animation)
		{
			var storyboard = new Storyboard();
			storyboard.Children.Add(animation);

			Storyboard.SetTarget(animation, lever);
			Storyboard.SetTargetProperty(animation, new PropertyPath("RenderTransform.X"));

			return storyboard;
		}
	}

	protected override void OnChecked(RoutedEventArgs e)
	{
		IsChecked = true;
		DoIsCheckedChanged();

		base.OnChecked(e);
	}

	protected override void OnUnchecked(RoutedEventArgs e)
	{
		IsChecked = false;
		DoIsCheckedChanged();

		base.OnUnchecked(e);
	}

}

