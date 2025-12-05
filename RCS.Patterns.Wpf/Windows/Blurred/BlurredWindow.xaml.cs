using OKB.Services.Native;
using OKB.Services.Native.Windows;
using System.Windows;

namespace OKB.Windows.Blurred
{
	public partial class BlurredWindow : WindowBase
	{
		readonly IWindowService _windowService;

		public BlurredWindow(IWindowService windowService)
		{
			_windowService = windowService ?? throw new ArgumentNullException(nameof(windowService));

			InitializeComponent();
			Loaded += BlurredWindow_Loaded;
		}

		void BlurredWindow_Loaded(object sender, RoutedEventArgs e)
		{
			Loaded -= BlurredWindow_Loaded;
			_windowService.ApplyBackgroundBlur(this, AccentState.ACCENT_ENABLE_BLURBEHIND);
		}
	}
}
