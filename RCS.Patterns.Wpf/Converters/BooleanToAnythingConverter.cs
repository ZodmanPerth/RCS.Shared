using System.Globalization;
using System.Windows.Data;

#nullable disable

namespace Patterns.Converters;

/// <summary>Converts boolean values to preset values of any type.</summary>
public class BooleanToAnythingConverter : IValueConverter
{
	/// <summary>The value to return when the value is false</summary>
	public object FalseValue { get; set; }

	/// <summary>The value to return when the value is true</summary>
	public object TrueValue { get; set; }


	object _nullValue;
	/// <summary>The value to return when the value is null</summary>
	public object NullValue
	{
		get
		{
			return _nullValue;
		}
		set
		{
			_nullValue = value;
			IsNullValueCheckRequired = true;
		}
	}

	/// <summary>Whether the value should be checked for Null.  Automatically set if NullValue is set.</summary>
	public bool IsNullValueCheckRequired { get; set; }



	//// IValueConverter


	public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
	{
		if (IsNullValueCheckRequired && value is null) return NullValue;
		if (value is not null && value is bool && (bool)value) return TrueValue;
		return FalseValue;
	}

	public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
	{
		if (IsNullValueCheckRequired && object.Equals(value, NullValue)) return null;
		if (object.Equals(value, TrueValue)) return true;
		if (object.Equals(value, FalseValue)) return false;
		return value;  //Can't convert.  Do a soft return to try not to break the application.
	}

}
