using System.Globalization;
using System.Windows.Data;

namespace Patterns.Converters;

/// <summary>Returns the contents of an attribute <see cref="AttributeName"/> on the property of the bound value passed in the parameter.</summary>
/// <remarks>
/// <see cref="AttributeName"/> defaults to "Description"<br></br>
/// <br></br>
/// Example: 
/// To get the descirption attribute of "myObject.myProperty", bind to "myObject" and pass "myProperty" in the converter parameter.<br></br>
/// </remarks>
public class AttributeValueConverter : IValueConverter
{
	/// <summary>The name of the attribute (without the "Attribute" suffix)</summary>
	public string AttributeName { get; set; } = "Description";

	public object? Convert(object value, Type targetType, object parameter, CultureInfo culture)
	{
		if (value is null)
			return value;

		if (parameter is null || parameter.ToString()!.IsNullOrWhitespace())
			return value;
		var propertyName = parameter.ToString()!;

		// Get the property from the passed value
		var propertyInfo = value.GetType().GetProperty(propertyName);
		if (propertyInfo is null)
			return value;

		// Get the attribute with the correct attibuteName from the property
		var attributeValue = GetAttributeValue();
		if (attributeValue is null)
			return value;

		return attributeValue;


		////// Local Functions


		string? GetAttributeValue()
		{
			// Get the attribute from the property
			var attribute = propertyInfo.GetCustomAttributes(inherit: false)
				.FirstOrDefault(_ => _.GetType().Name == $"{AttributeName}Attribute");
			if (attribute is null)
				return null;

			// Get the property containing the value from the attribute
			var attributeValueProperty = attribute.GetType().GetProperty(AttributeName);
			if (attributeValueProperty is null)
				return null;

			// Get the value
			return attributeValueProperty.GetValue(attribute)?.ToString();
		}
	}

	public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) =>
		throw new NotImplementedException();
}
