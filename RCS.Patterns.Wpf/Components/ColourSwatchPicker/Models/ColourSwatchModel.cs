using CommunityToolkit.Mvvm.ComponentModel;
using System.Windows.Media;

namespace OKB.Components.ColourSwatchPicker;

/// <summary>The colour model used in the <see cref="ColourSwatchPickerPopup"/></summary>
public partial class ColourSwatchModel : ObservableObject
{

	/// <summary>Set by hover</summary>
	[ObservableProperty]
	public partial ColourSwatch? Hover { get; set; }
	partial void OnHoverChanged(ColourSwatch? value) =>
		SynchroniseApplicable();

	[ObservableProperty]
	/// <summary>Set by selection</summary>
	public partial ColourSwatch? Selected { get; set; }
	partial void OnSelectedChanged(ColourSwatch? value) =>
		SynchroniseApplicable();


	/// <summary>Set by Show()</summary>
	public ColourSwatch? Default { get; set; }


	/// <summary>
	/// The applicable <see cref="ColourSwatch"/>:<br></br>
	/// * When hovering an item, the hovered <see cref="ColourSwatch"/><br></br>
	/// * When an item is selected, the selected <see cref="ColourSwatch"/><br></br>
	/// * Otherwise, the default <see cref="ColourSwatch"/>
	/// </summary>
	[ObservableProperty]
	public partial ColourSwatch? Applicable { get; set; }



	//// Helpers


	void SynchroniseApplicable()
	{
		if (Hover is not null)
			Applicable = Hover;
		else if (Selected is not null)
			Applicable = Selected;
		else
			Applicable = Default;
	}



	//// Actions


	/// <summary>Reset the model in preparation to show</summary>
	/// <remarks>Use this to set the current brush as the default</remarks>
	public void Reset(Brush defaultBrush)
	{
		if (defaultBrush is null) throw new ArgumentNullException(nameof(defaultBrush));

		var defaultColourSwatch = new ColourSwatch("default", defaultBrush);

		Default = defaultColourSwatch;
		Hover = null;
		Selected = null;
	}

}
