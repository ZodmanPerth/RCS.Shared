using System.Windows.Media;

namespace System.Windows;

public static partial class DependencyObjectExtensions
{
	/// <summary>Finds the visual parent with the required type</summary>
	public static T? FindVisualParent<T>(this UIElement element) where T : UIElement
	{
		var parent = element;
		while (parent is not null)
		{
			if (parent is T) { return parent as T; }
			parent = VisualTreeHelper.GetParent(parent) as UIElement;
		}
		return null;
	}

	/// <summary>Walk object's children, finding the child with type <typeparamref name="T"/>.
	public static T? FindVisualChild<T>(this DependencyObject root) where T : DependencyObject
	{
		for (int i = 0; i < VisualTreeHelper.GetChildrenCount(root); i++)
		{
			DependencyObject child = VisualTreeHelper.GetChild(root, i);
			if (child is not null && child is T)
				return (T)child;
			else
			{
				T? childOfChild = FindVisualChild<T>(child!);
				if (childOfChild is not null) { return childOfChild; }
			}
		}

		return null;
	}

	/// <summary>Returns a list of all children in the visual tree of a DependencyObject with a specified type.</summary>
	public static List<T> FindVisualChildren<T>(this DependencyObject root) where T : DependencyObject
	{
		List<T> result = new List<T>();
		for (int i = 0; i < VisualTreeHelper.GetChildrenCount(root); i++)
		{
			DependencyObject child = VisualTreeHelper.GetChild(root, i);
			if (child is not null && child is T)
			{
				result.Add((T)child);
			}
			else
			{
				List<T> childrenOfChild = FindVisualChildren<T>(child!);
				if (childrenOfChild.Count != 0) { result.AddRange(childrenOfChild); }
			}
		}

		return result;
	}

	/// <summary>Walks the visual tree of the root object, finding the child with type T by name.</summary>
	public static T? FindVisualChildByName<T>(this DependencyObject root, string childName) where T : DependencyObject
	{
		if (root == null)
			return null;

		if (childName.IsNullOrWhitespace())
			return null;

		int childrenCount = VisualTreeHelper.GetChildrenCount(root);
		for (int i = 0; i < childrenCount; i++)
		{
			var child = VisualTreeHelper.GetChild(root, i);

			var feChild = child as FrameworkElement;
			if (feChild != null && feChild.Name == childName)
				return (T)child;

			var result = FindVisualChildByName<T>(child, childName);
			if (result != null)
				return result;
		}

		return null;
	}
}

