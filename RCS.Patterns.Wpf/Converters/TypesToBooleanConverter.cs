using System.Globalization;
using System.Windows.Data;

#nullable disable

namespace Patterns.Converters;

/// <summary>If the bound object is of a type listed in <see cref="IncludedTypes"/> return the specified <see cref="Value"/></summary>
public class TypesToBooleanConverter : IValueConverter
{
	List<Type> _includedTypes = null;

	public Type[] IncludedTypes { get; set; }
	public bool Value { get; set; }

	public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
	{
		if (_includedTypes is null)
			_includedTypes = new List<Type>(IncludedTypes);

		if (_includedTypes.Contains(value.GetType()))
			return Value;

		return !Value;
	}

	public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
	{
		throw new NotImplementedException();
	}
}


