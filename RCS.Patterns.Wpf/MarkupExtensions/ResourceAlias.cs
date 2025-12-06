using System.Windows;
using System.Windows.Markup;
using System.Windows.Media;

namespace RCS.Patterns.Wpf.MarkupExtensions;


public class HorizontalAlignmentAlias : ResourceAlias<HorizontalAlignment> { /* nada */ }
public class VerticalAlignmentAlias : ResourceAlias<VerticalAlignment> { /* nada */ }
public class MarginAlias : ResourceAlias<Thickness> { /* nada */ }
public class ThicknessAlias : ResourceAlias<Thickness> { /* nada */ }

public class FontFamilyAlias : ResourceAlias<FontFamily> { /* nada */ }
public class FontWeightAlias : ResourceAlias<FontWeight> { /* nada */ }

public class DoubleAlias : ResourceAlias<double> { /* nada */ }


/// <summary>Alias a resourceKey from another resource</summary>
/// <remarks>
/// WPF doesn't let you reference other resource keys when overriding a resource key.  This class helps you do that.
/// However you can't reference it directly in XAML.  You need to add a non-generic instance for each return type.
/// </remarks>
[MarkupExtensionReturnType(typeof(object))]
public class ResourceAlias<T> : MarkupExtension
{
	public string? SourceKey { get; set; }

	public override object? ProvideValue(IServiceProvider serviceProvider)
	{
		if (string.IsNullOrEmpty(SourceKey))
			return default(T);

		var valueTarget = serviceProvider.GetService(typeof(IProvideValueTarget)) as IProvideValueTarget;
		var targetObject = valueTarget?.TargetObject as FrameworkElement;

		object? resolved = null;

		if (targetObject != null)
			resolved = targetObject.TryFindResource(SourceKey);

		if (resolved == null)
			resolved = Application.Current.TryFindResource(SourceKey);

		return resolved is T typed ? typed : default;
	}
}
