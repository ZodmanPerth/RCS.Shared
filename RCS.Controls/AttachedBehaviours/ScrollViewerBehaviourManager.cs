using Microsoft.Xaml.Behaviors;
using System.Windows;
using System.Windows.Controls;

namespace OKB.Controls.Behaviours;

/// <summary>Attaches custom attached behaviours</summary>
public static class ScrollViewerBehaviourManager
{
	public static bool GetIsAttachHorizontalScrollingBehaviour(DependencyObject element)
		=> (bool)element.GetValue(IsAttachHorizontalScrollingBehaviourProperty);

	public static void SetIsAttachHorizontalScrollingBehaviour(DependencyObject element, bool value)
		=> element.SetValue(IsAttachHorizontalScrollingBehaviourProperty, value);

	public static readonly DependencyProperty IsAttachHorizontalScrollingBehaviourProperty = DependencyProperty.RegisterAttached
	(
		"IsAttachHorizontalScrollingBehaviour",
		typeof(bool),
		typeof(ScrollViewerBehaviourManager),
		new FrameworkPropertyMetadata(defaultValue: false, propertyChangedCallback: OnIsAttachHorizontalScrollingBehaviourChanged)
	);
	static void OnIsAttachHorizontalScrollingBehaviourChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
	{
		if (d is not ScrollViewer scrollViewer)
			return;

		var behaviors = Interaction.GetBehaviors(scrollViewer);

		// Remove existing instance (if we're turning off or on)
		var existingBehaviours = behaviors.OfType<ScrollViewerHorizontalScrollingBehaviour>().FirstOrDefault();
		if (existingBehaviours is not null)
			behaviors.Remove(existingBehaviours);

		// Add if toggled on
		if ((bool)e.NewValue)
			behaviors.Add(new ScrollViewerHorizontalScrollingBehaviour());
	}
}
