using System.Windows.Controls.Primitives;
using System.Windows.Media;

namespace OKB.Components.ColourSwatchPicker;

// DESCRIPTION
// A popup that displays a palette of colour swatches for selection.
// Suitable for colour picking scenarios where the colour can be applied live to a property.
//
// FEATURES
// * Hook up to a PlacementTarget and call `Show` with the current brush (the Default) to show the popup.
// * Listen to ApplicableColourSwatchChanged to get updates when the Applicable swatch changes:
//   * Hovering a swatch
//   * Selecting a swatch
//   * Not hovering or selecting (reverts to Default swatch)
// * The popup closes automatically when a swatch is selected.
//
// USAGE
//
// void ShowColourSwatchPopup(ColourSwatchPickerPopup? popup, UIElement placementTarget)
// {
// 	bool isInitialisePopup = popup is null;
// 	if (isInitialisePopup)
// 		CreateColourSwatchPopup();
// 
// 	popup!.Show(/* brush source */);
// 
// 	return;
// 
// 
// 	//// Local Functions
// 
// 
// 	void CreateColourSwatchPopup()
// 	{
// 		popup = _colourSwatchPickerPopupManagerFactory();
// 		popup.PlacementTarget = placementTarget;
// 
// 		popup.ApplicableColourSwatchChanged += (_) =>
// 			/* update brush source */
// 	}
// }


/// <summary>A popup that displays colour swatches for selection</summary>
public class ColourSwatchPickerPopup : Popup
{
	/// <summary>Raised when the applicable <see cref="ColourSwatch"/> changes</summary>
	/// <remarks>This is the event that contains the state of the colour selection (hover, selected, default)</remarks>
	public event Action<ColourSwatch>? ApplicableColourSwatchChanged;


	readonly Func<ColourSwatchPickerView> _colourSwatchPickerViewFactory;


	ColourSwatchPickerView? _pickerView;



	//// Lifecycle


	public ColourSwatchPickerPopup(Func<ColourSwatchPickerView> colourSwatchPickerViewFactory)
	{
		_colourSwatchPickerViewFactory = colourSwatchPickerViewFactory ?? throw new ArgumentNullException(nameof(colourSwatchPickerViewFactory));

		// defaults
		Placement = PlacementMode.Left;
		HorizontalOffset = 4;
		StaysOpen = false;
		AllowsTransparency = true;
	}



	//// Actions


	public void Show(Brush defaultBrush)
	{
		if (defaultBrush is null) throw new ArgumentNullException(nameof(defaultBrush));

		if (_pickerView is null)
			CreateView();

		_pickerView!.ColourSwatch.Reset(defaultBrush);

		this.IsOpen = true;

		return;


		//// Local Functions


		void CreateView()
		{
			_pickerView = _colourSwatchPickerViewFactory();

			_pickerView.ColourSwatch.PropertyChanged += (s, e) =>
			{
				if (e.PropertyName == nameof(ColourSwatchModel.Selected))
					this.IsOpen = false;

				else if (e.PropertyName == nameof(ColourSwatchModel.Applicable))
					ApplicableColourSwatchChanged?.Invoke(_pickerView.ColourSwatch.Applicable!);
			};

			this.Child = _pickerView;
		}
	}
}
