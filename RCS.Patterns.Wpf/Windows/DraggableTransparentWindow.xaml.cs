using OKB.Services.Native.Windows;
using System.Windows.Input;

namespace OKB.Windows;

public partial class DraggableTransparentWindow : WindowBase
{


	//// Lifecycle


	public DraggableTransparentWindow(IWindowService windowService)
	{
		InitializeComponent();

		Loaded += (s, e) => windowService.HideWindowFromTaskSwitcher(this);
	}



	//// Event Handlers


	void root_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
	{
		this.DragMove();
	}
}
