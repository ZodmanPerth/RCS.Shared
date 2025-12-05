using System.Windows.Data;

namespace System.Windows;

public static partial class DependencyObjectExtensions
{
	/// <summary>Creates a binding and sets it to the object.</summary>
	/// <remarks>Returns the same dependency object.</remarks>
	public static DependencyObject CreateAndSetBinding
	(
		this DependencyObject dependencyObject,
		DependencyProperty dependencyProperty,
		object source,
		BindingMode mode = BindingMode.OneWay
	)
	{
		var binding = new Binding();
		binding.Source = source;
		binding.Mode = mode;

		BindingOperations.SetBinding(dependencyObject, dependencyProperty, binding);

		return dependencyObject;
	}

}

