using OKB.Models;
using OKB.Services.Native.Displays;
using OKB.Windows.PersistedState;

#nullable disable

namespace OKB.Windows;

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
