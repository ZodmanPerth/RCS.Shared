using System.Windows.Controls;
using System.Windows.Input;

namespace OKB.Components.ColourSwatchPicker;

public partial class ColourSwatchPickerView : UserControl
{
	public ColourSwatchPickerViewModel ViewModel { get; }

	public ColourSwatchModel ColourSwatch { get; }



	//// Lifecycle


	public ColourSwatchPickerView(ColourSwatchPickerViewModel viewModel)
	{
		InitializeComponent();
		ViewModel = viewModel ?? throw new ArgumentNullException(nameof(viewModel));
		DataContext = this;

		ColourSwatch = new();
	}



	//// Event Handlers


	void ListBoxItem_MouseEnter(object sender, MouseEventArgs e)
	{
		if (sender is not ListBoxItem item)
			return;

		if (item.DataContext is not ColourSwatch hoverSwatch)
			return;

		ColourSwatch.Hover = hoverSwatch;
	}

	void lbColours_MouseLeave(object sender, MouseEventArgs e) =>
		ColourSwatch.Hover = null;
}