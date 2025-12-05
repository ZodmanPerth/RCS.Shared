using System.Globalization;
using System.Windows.Data;

namespace Patterns.Converters;

/// <summary>Maps the index to the set of predefined results</summary>
public class IndexMappingConverter : IValueConverter
{
	public List<object> Results { get; set; } = new();

	public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
	{
		if (!Results.Any())
			return value;

		if (Results.Count == 1)
			return Results[0];

		if (value is not int index)
			return value;

		var resultIndex = index % Results.Count;
		return Results[resultIndex];

	}

	public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) => 
		throw new NotImplementedException();
}
