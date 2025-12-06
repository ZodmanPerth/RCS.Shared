using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace RCS.Theme.AttachedProperties;

/// <summary>Generates a simple hierarchical data template</summary>
public class TreeViewHierarchyAttachedProperties
{
	/// <summary>The path to the children</summary>
	public static readonly DependencyProperty ChildrenPathProperty = DependencyProperty.RegisterAttached
	(
		"ChildrenPath",
		typeof(string),
		typeof(TreeViewHierarchyAttachedProperties),
		new FrameworkPropertyMetadata(defaultValue: default, flags: FrameworkPropertyMetadataOptions.AffectsRender, propertyChangedCallback: OnChildrenPathChanged)
	);

	public static void SetChildrenPath(DependencyObject element, string value) =>
		element.SetValue(ChildrenPathProperty, value);

	public static string GetChildrenPath(DependencyObject element) =>
		(string)element.GetValue(ChildrenPathProperty);

	static void OnChildrenPathChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) =>
		DoPathChanged(d);


	/// <summary>The path to the display property</summary>
	public static readonly DependencyProperty DisplayPathProperty = DependencyProperty.RegisterAttached
	(
		"DisplayPath",
		typeof(string),
		typeof(TreeViewHierarchyAttachedProperties),
		new FrameworkPropertyMetadata(defaultValue: default, flags: FrameworkPropertyMetadataOptions.AffectsRender, propertyChangedCallback: OnDisplayPathChanged)
	);

	public static void SetDisplayPath(DependencyObject element, string value) =>
	element.SetValue(DisplayPathProperty, value);

	public static string GetDisplayPath(DependencyObject element) =>
		(string)element.GetValue(DisplayPathProperty);

	static void OnDisplayPathChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) =>
		DoPathChanged(d);



	//// Helpers


	/// <summary>
	/// Sets up a <see cref="HierarchicalDataTemplate"/> on the ChildPath with a simple
	/// <see cref="TextBlock"/> for the DisplayPath.
	/// </summary>
	static void DoPathChanged(DependencyObject d)
	{
		//	<HierarchicalDataTemplate ItemsSource="{Binding {ChildrenPath}, Mode=OneWay}" >
		//		<TextBlock Text = "{Binding {DisplayPath}, Mode=OneTime}" />
		//	</ HierarchicalDataTemplate >

		if (d is not TreeView tree)
			return;

		var childrenPath = GetChildrenPath(tree);
		if (childrenPath.IsNullOrWhitespace())
			return;

		var displayPath = GetDisplayPath(tree);
		if (displayPath.IsNullOrWhitespace())
			return;

		var template = new HierarchicalDataTemplate
		{
			ItemsSource = new Binding(childrenPath)
		};

		var textBlockFactory = new FrameworkElementFactory(typeof(TextBlock));
		textBlockFactory.SetBinding(TextBlock.TextProperty, new Binding(displayPath));
		template.VisualTree = textBlockFactory;

		tree.ItemTemplate = template;
	}
}

