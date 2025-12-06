using CommunityToolkit.Mvvm.ComponentModel;
using System.Windows;
using System.Windows.Media;

namespace RCS.Components.ColourSwatchPicker;

public partial class ColourSwatchPickerViewModel : ObservableObject
{
	public List<ColourSwatch> AvailableColours { get; }



	//// Lifecycle


	public ColourSwatchPickerViewModel()
	{
		var designBrushResourceDictionary = new ResourceDictionary
		{
			Source = new Uri("pack://application:,,,/RCS.Theme;component/Design/Design.Brushes.xaml", UriKind.RelativeOrAbsolute)
		};

		AvailableColours =
		[
			new("White", Brushes.White),
			new("Grey100", CreateBrushFromHex("#EEE")),
			new("Grey300", CreateBrushFromHex("#CCC")),
			new("Grey500", CreateBrushFromHex("#AAA")),
			new("Grey600", CreateBrushFromHex("#888")),
			new("Grey700", CreateBrushFromHex("#666")),
			new("Grey800", CreateBrushFromHex("#444")),
			new("Black", Brushes.Black),
			CreateFromResourceDesignBrush("Design.Brush.Beige200"),
			CreateFromResourceDesignBrush("Design.Brush.Beige300"),
			CreateFromResourceDesignBrush("Design.Brush.Beige400"),
			CreateFromResourceDesignBrush("Design.Brush.Beige500"),
		];


		return;


		//// Local Functions


		Brush CreateBrushFromHex(string hex) =>
			new SolidColorBrush((Color)ColorConverter.ConvertFromString(hex));

		ColourSwatch CreateFromResourceDesignBrush(string key)
		{
			if (designBrushResourceDictionary[key] is not Brush brush)
				throw new($"Can't find resource {key}");

			var displayText = key.Replace("Design.Brush.", string.Empty);

			return new ColourSwatch(displayText, brush);
		}
	}
}
