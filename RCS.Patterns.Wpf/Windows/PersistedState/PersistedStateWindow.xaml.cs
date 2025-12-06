using RCS.Models;
using RCS.Services.Native.Displays;
using RCS.Windows.PersistedState;

#nullable disable

namespace RCS.Windows;

public partial class PersistedStateWindow : PersistedStateWindowBase
{
	public PersistedStateWindow
	(
		// services
		IAppSettingsNative appSettings,
		INativeDisplayService nativeDisplayService,

		// data
		string windowSettingsName
	)
		: base(appSettings, nativeDisplayService, windowSettingsName)
	{
		InitializeComponent();
	}
}
