using System.Windows;

namespace System;

public static class BooleanExtensions
{
	/// <summary>Returns <see cref="Visibility.Visibility"/> when true, <see cref="Visibility.Collapsed"/> when false</summary>
	public static Visibility ToVisibleWhenTrue(this bool value) =>
		value ? Visibility.Visible : Visibility.Collapsed;
}

